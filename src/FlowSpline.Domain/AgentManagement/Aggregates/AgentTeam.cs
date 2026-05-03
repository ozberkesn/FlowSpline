using FlowSpline.Domain.AgentManagement.Events;
using FlowSpline.Domain.Common;

namespace FlowSpline.Domain.AgentManagement.Aggregates
{
    public class AgentTeam : AggregateRoot
    {
        private readonly HashSet<Guid> _memberIds = new();

        public Guid Id { get; private set; }
        public string Name { get; private set; } = null!;
        public Guid SupervisorId { get; private set; }
        public IReadOnlyCollection<Guid> MemberIds => _memberIds;

        private AgentTeam() { }

        public AgentTeam(string name, Guid supervisorId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Team name is required.");

            var trimmedName = name.Trim();
            if (trimmedName.Length < 3)
                throw new ArgumentException("Team name must be at least 3 characters.");

            if (supervisorId == Guid.Empty)
                throw new ArgumentException("Supervisor is required.");

            Id = Guid.NewGuid();
            Name = trimmedName;
            SupervisorId = supervisorId;

            AddDomainEvent(new AgentTeamCreatedEvent(Id, supervisorId));
        }

        public void AddMember(Guid agentId)
        {
            if (agentId == Guid.Empty)
                throw new ArgumentException("Agent ID is required.");

            if (agentId == SupervisorId)
                throw new InvalidOperationException("Supervisor is already a team member implicitly.");

            if (!_memberIds.Add(agentId))
                throw new InvalidOperationException("Agent is already a member of this team.");

            AddDomainEvent(new MemberAddedEvent(Id, agentId));
        }

        public void RemoveMember(Guid agentId)
        {
            if (agentId == Guid.Empty)
                throw new ArgumentException("Agent ID is required.");

            if (agentId == SupervisorId)
                throw new InvalidOperationException("Supervisor cannot be removed from the team.");

            _memberIds.Remove(agentId);
            AddDomainEvent(new MemberRemovedEvent(Id, agentId));
        }

        public void ChangeSupervisor(Guid agentId)
        {
            if (agentId == Guid.Empty)
                throw new ArgumentException("Agent ID is required.");

            if (agentId == SupervisorId)
                throw new InvalidOperationException("Agent is already the supervisor.");

            if (!_memberIds.Contains(agentId))
                throw new InvalidOperationException("New supervisor must be an existing team member.");

            SupervisorId = agentId;
            AddDomainEvent(new SupervisorChangedEvent(Id, agentId));
        }
    }
}
