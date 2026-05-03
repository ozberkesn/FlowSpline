namespace FlowSpline.Domain.AgentManagement.Events
{
    internal class AgentDeactivatedEvent
    {
        public Guid AgentId { get; }

        public AgentDeactivatedEvent(Guid agentId)
        {
            AgentId = agentId;
        }
    }
}
