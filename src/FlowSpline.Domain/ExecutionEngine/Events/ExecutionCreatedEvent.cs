namespace FlowSpline.Domain.ExecutionEngine.Events
{
    internal class ExecutionCreatedEvent
    {
        public Guid ExecutionId { get; }
        public Guid AgentId { get; }

        public ExecutionCreatedEvent(Guid executionId, Guid agentId)
        {
            ExecutionId = executionId;
            AgentId = agentId;
        }
    }
}
