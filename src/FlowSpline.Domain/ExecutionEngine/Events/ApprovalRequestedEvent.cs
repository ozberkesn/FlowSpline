namespace FlowSpline.Domain.ExecutionEngine.Events
{
    internal class ApprovalRequestedEvent
    {
        public Guid ExecutionId { get; }

        public ApprovalRequestedEvent(Guid executionId)
        {
            ExecutionId = executionId;
        }
    }
}
