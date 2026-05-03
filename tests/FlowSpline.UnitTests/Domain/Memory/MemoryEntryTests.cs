using FlowSpline.Domain.Memory.Aggregates;

namespace FlowSpline.UnitTests.Domain.Memory;

public class MemoryEntryTests
{
    [Fact]
    public void CreateEntry_WithValidData_ShouldCreateEntry()
    {
        var agentId = Guid.NewGuid();
        var sessionId = Guid.NewGuid();

        var entry = new MemoryEntry(agentId, sessionId, "user_name", "Alice");

        Assert.NotEqual(Guid.Empty, entry.Id);
        Assert.Equal(agentId, entry.AgentId);
        Assert.Equal(sessionId, entry.SessionId);
        Assert.Equal("user_name", entry.Key);
        Assert.Equal("Alice", entry.Value);
        Assert.Null(entry.ExpiresAt);
    }

    [Fact]
    public void CreateEntry_WithEmptyAgentId_ShouldThrow()
    {
        Assert.Throws<ArgumentException>(() =>
            new MemoryEntry(Guid.Empty, Guid.NewGuid(), "key", "value"));
    }

    [Fact]
    public void CreateEntry_WithEmptyKey_ShouldThrow()
    {
        Assert.Throws<ArgumentException>(() =>
            new MemoryEntry(Guid.NewGuid(), Guid.NewGuid(), "", "value"));
    }

    [Fact]
    public void UpdateValue_WithValidValue_ShouldUpdateValue()
    {
        var entry = CreateEntry();

        entry.UpdateValue("Bob");

        Assert.Equal("Bob", entry.Value);
    }

    [Fact]
    public void UpdateValue_WithEmpty_ShouldThrow()
    {
        var entry = CreateEntry();

        Assert.Throws<ArgumentException>(() => entry.UpdateValue(""));
    }

    [Fact]
    public void Expire_ShouldSetExpiresAt()
    {
        var entry = CreateEntry();

        entry.Expire();

        Assert.NotNull(entry.ExpiresAt);
    }

    [Fact]
    public void Expire_WhenAlreadyExpired_ShouldNotRaiseSecondEvent()
    {
        var entry = CreateEntry();
        entry.Expire();
        var firstExpiresAt = entry.ExpiresAt;
        entry.ClearDomainEvents();

        entry.Expire();

        Assert.Empty(entry.DomainEvents);
        Assert.Equal(firstExpiresAt, entry.ExpiresAt);
    }

    [Fact]
    public void CreateEntry_WithExpiry_ShouldStoreExpiry()
    {
        var expiry = DateTimeOffset.UtcNow.AddHours(1);

        var entry = new MemoryEntry(Guid.NewGuid(), Guid.NewGuid(), "key", "value", expiry);

        Assert.Equal(expiry, entry.ExpiresAt);
    }

    [Fact]
    public void CreateEntry_ShouldRaiseDomainEvent()
    {
        var entry = CreateEntry();

        Assert.Single(entry.DomainEvents);
    }

    private static MemoryEntry CreateEntry() =>
        new(Guid.NewGuid(), Guid.NewGuid(), "user_name", "Alice");
}
