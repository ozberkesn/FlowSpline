namespace FlowSpline.Domain.ExecutionEngine.Events
{
    internal class ExecutionApprovedEvent
    {
        public Guid ExecutionId { get; }

        public ExecutionApprovedEvent(Guid executionId)
        {
            ExecutionId = executionId;
        }
    }
}
