namespace FlowSpline.Domain.Memory.Events
{
    internal class MemoryEntryCreatedEvent
    {
        public Guid EntryId { get; }
        public Guid AgentId { get; }
        public Guid SessionId { get; }

        public MemoryEntryCreatedEvent(Guid entryId, Guid agentId, Guid sessionId)
        {
            EntryId = entryId;
            AgentId = agentId;
            SessionId = sessionId;
        }
    }
}
