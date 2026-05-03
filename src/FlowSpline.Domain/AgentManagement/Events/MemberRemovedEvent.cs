namespace FlowSpline.Domain.AgentManagement.Events
{
    internal class MemberRemovedEvent
    {
        public Guid TeamId { get; }
        public Guid AgentId { get; }

        public MemberRemovedEvent(Guid teamId, Guid agentId)
        {
            TeamId = teamId;
            AgentId = agentId;
        }
    }
}
