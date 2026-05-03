namespace FlowSpline.Domain.AgentManagement.Events
{
    internal class MemberAddedEvent
    {
        public Guid TeamId { get; }
        public Guid AgentId { get; }

        public MemberAddedEvent(Guid teamId, Guid agentId)
        {
            TeamId = teamId;
            AgentId = agentId;
        }
    }
}
