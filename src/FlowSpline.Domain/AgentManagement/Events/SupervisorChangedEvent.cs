namespace FlowSpline.Domain.AgentManagement.Events
{
    internal class SupervisorChangedEvent
    {
        public Guid TeamId { get; }
        public Guid NewSupervisorId { get; }

        public SupervisorChangedEvent(Guid teamId, Guid newSupervisorId)
        {
            TeamId = teamId;
            NewSupervisorId = newSupervisorId;
        }
    }
}
