namespace FlowSpline.Domain.ExecutionEngine.Events
{
    internal class ExecutionCompletedEvent
    {
        public Guid ExecutionId { get; }

        public ExecutionCompletedEvent(Guid executionId)
        {
            ExecutionId = executionId;
        }
    }
}
