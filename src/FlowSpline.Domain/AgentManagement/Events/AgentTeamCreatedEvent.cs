namespace FlowSpline.Domain.AgentManagement.Events
{
    internal class AgentTeamCreatedEvent
    {
        public Guid TeamId { get; }
        public Guid SupervisorId { get; }

        public AgentTeamCreatedEvent(Guid teamId, Guid supervisorId)
        {
            TeamId = teamId;
            SupervisorId = supervisorId;
        }
    }
}
