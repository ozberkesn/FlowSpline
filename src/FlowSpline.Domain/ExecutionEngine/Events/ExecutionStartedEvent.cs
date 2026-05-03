namespace FlowSpline.Domain.ExecutionEngine.Events
{
    internal class ExecutionStartedEvent
    {
        public Guid ExecutionId { get; }

        public ExecutionStartedEvent(Guid executionId)
        {
            ExecutionId = executionId;
        }
    }
}
