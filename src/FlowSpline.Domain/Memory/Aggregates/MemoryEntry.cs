using FlowSpline.Domain.Common;
using FlowSpline.Domain.Memory.Events;

namespace FlowSpline.Domain.Memory.Aggregates
{
    public class MemoryEntry : AggregateRoot
    {
        public Guid Id { get; private set; }
        public Guid AgentId { get; private set; }
        public Guid SessionId { get; private set; }
        public string Key { get; private set; } = null!;
        public string Value { get; private set; } = null!;
        public DateTimeOffset CreatedAt { get; private set; }
        public DateTimeOffset? ExpiresAt { get; private set; }

        private MemoryEntry() { }

        public MemoryEntry(Guid agentId, Guid sessionId, string key, string value, DateTimeOffset? expiresAt = null)
        {
            if (agentId == Guid.Empty)
                throw new ArgumentException("Agent ID is required.");

            if (sessionId == Guid.Empty)
                throw new ArgumentException("Session ID is required.");

            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Key is required.");

            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Value is required.");

            Id = Guid.NewGuid();
            AgentId = agentId;
            SessionId = sessionId;
            Key = key;
            Value = value;
            CreatedAt = DateTimeOffset.UtcNow;
            ExpiresAt = expiresAt;

            AddDomainEvent(new MemoryEntryCreatedEvent(Id, AgentId, SessionId));
        }

        public void UpdateValue(string newValue)
        {
            if (string.IsNullOrWhiteSpace(newValue))
                throw new ArgumentException("Value is required.");

            Value = newValue;
            AddDomainEvent(new MemoryEntryUpdatedEvent(Id));
        }

        public void Expire()
        {
            if (ExpiresAt.HasValue) return;
            ExpiresAt = DateTimeOffset.UtcNow;
            AddDomainEvent(new MemoryEntryExpiredEvent(Id));
        }
    }
}
