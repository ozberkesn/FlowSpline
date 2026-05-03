namespace FlowSpline.Domain.Memory.Events
{
    internal class MemoryEntryExpiredEvent
    {
        public Guid EntryId { get; }

        public MemoryEntryExpiredEvent(Guid entryId)
        {
            EntryId = entryId;
        }
    }
}
