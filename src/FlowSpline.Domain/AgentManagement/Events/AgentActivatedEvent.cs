namespace FlowSpline.Domain.AgentManagement.Events
{
    internal class AgentActivatedEvent
    {
        public Guid AgentId { get; }

        public AgentActivatedEvent(Guid agentId)
        {
            AgentId = agentId;
        }
    }
}
