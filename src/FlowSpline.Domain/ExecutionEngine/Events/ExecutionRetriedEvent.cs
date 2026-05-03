namespace FlowSpline.Domain.ExecutionEngine.Events
{
    internal class ExecutionRetriedEvent
    {
        public Guid ExecutionId { get; }
        public int RetryCount { get; }

        public ExecutionRetriedEvent(Guid executionId, int retryCount)
        {
            ExecutionId = executionId;
            RetryCount = retryCount;
        }
    }
}
