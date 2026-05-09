using System.Text.Json;
using FlowSpline.Application.Memory.Repositories;
using FlowSpline.Domain.Memory.Aggregates;
using StackExchange.Redis;

namespace FlowSpline.Persistence.Memory;

internal sealed class MemoryEntryRepository : IMemoryEntryRepository
{
    private readonly IConnectionMultiplexer _redis;

    public MemoryEntryRepository(IConnectionMultiplexer redis) => _redis = redis;

    private IDatabase Db => _redis.GetDatabase();

    private static string Key(Guid agentId, Guid sessionId, string key)
        => $"memory:{agentId}:{sessionId}:{key}";

    public async Task<MemoryEntry?> GetByKeyAsync(
        Guid agentId, Guid sessionId, string key, CancellationToken cancellationToken = default)
    {
        var value = await Db.StringGetAsync(Key(agentId, sessionId, key));
        if (value.IsNullOrEmpty) return null;

        var data = JsonSerializer.Deserialize<MemoryData>((string)value!);
        return data is null ? null : ToEntry(data);
    }

    public async Task<IReadOnlyList<MemoryEntry>> GetBySessionAsync(
        Guid agentId, Guid sessionId, CancellationToken cancellationToken = default)
    {
        var server = _redis.GetServer(_redis.GetEndPoints()[0]);
        var pattern = $"memory:{agentId}:{sessionId}:*";
        var keys = server.Keys(pattern: pattern).ToArray();

        if (keys.Length == 0) return [];

        var values = await Db.StringGetAsync(keys);
        var result = new List<MemoryEntry>(values.Length);

        foreach (var value in values)
        {
            if (value.IsNullOrEmpty) continue;
            var data = JsonSerializer.Deserialize<MemoryData>((string)value!);
            if (data is not null) result.Add(ToEntry(data));
        }

        return result;
    }

    public async Task AddAsync(MemoryEntry entry, CancellationToken cancellationToken = default)
    {
        var json = JsonSerializer.Serialize(FromEntry(entry));
        var redisKey = Key(entry.AgentId, entry.SessionId, entry.Key);

        if (entry.ExpiresAt.HasValue)
        {
            var ttl = entry.ExpiresAt.Value - DateTimeOffset.UtcNow;
            if (ttl <= TimeSpan.Zero) return;
            await Db.StringSetAsync(redisKey, json, ttl);
        }
        else
        {
            await Db.StringSetAsync(redisKey, json);
        }
    }

    public async Task UpdateAsync(MemoryEntry entry, CancellationToken cancellationToken = default)
    {
        var redisKey = Key(entry.AgentId, entry.SessionId, entry.Key);
        var json = JsonSerializer.Serialize(FromEntry(entry));

        var existingTtl = await Db.KeyTimeToLiveAsync(redisKey);
        if (existingTtl.HasValue)
            await Db.StringSetAsync(redisKey, json, existingTtl.Value);
        else
            await Db.StringSetAsync(redisKey, json);
    }

    private static MemoryEntry ToEntry(MemoryData data)
        => new(data.AgentId, data.SessionId, data.Key, data.Value, data.ExpiresAt);

    private static MemoryData FromEntry(MemoryEntry entry)
        => new(entry.AgentId, entry.SessionId, entry.Key, entry.Value, entry.CreatedAt, entry.ExpiresAt);

    private sealed record MemoryData(
        Guid AgentId,
        Guid SessionId,
        string Key,
        string Value,
        DateTimeOffset CreatedAt,
        DateTimeOffset? ExpiresAt);
}
