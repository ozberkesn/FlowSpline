namespace FlowSpline.Domain.Memory.Events
{
    internal class MemoryEntryUpdatedEvent
    {
        public Guid EntryId { get; }

        public MemoryEntryUpdatedEvent(Guid entryId)
        {
            EntryId = entryId;
        }
    }
}
