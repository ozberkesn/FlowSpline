namespace FlowSpline.Domain.AgentManagement.Events
{
    internal class AgentCreatedEvent
    {
        public Guid AgentId { get; }

        public AgentCreatedEvent(Guid agentId)
        {
            AgentId = agentId;
        }
    }
}
