namespace FlowSpline.Domain.ExecutionEngine.Events
{
    internal class ExecutionFailedEvent
    {
        public Guid ExecutionId { get; }
        public string Reason { get; }

        public ExecutionFailedEvent(Guid executionId, string reason)
        {
            ExecutionId = executionId;
            Reason = reason;
        }
    }
}
