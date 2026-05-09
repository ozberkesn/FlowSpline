using FlowSpline.Domain.Memory.Aggregates;

namespace FlowSpline.Application.Memory.Repositories;

public interface IMemoryEntryRepository
{
    Task<MemoryEntry?> GetByKeyAsync(Guid agentId, Guid sessionId, string key, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<MemoryEntry>> GetBySessionAsync(Guid agentId, Guid sessionId, CancellationToken cancellationToken = default);
    Task AddAsync(MemoryEntry entry, CancellationToken cancellationToken = default);
    Task UpdateAsync(MemoryEntry entry, CancellationToken cancellationToken = default);
}
