using FlowSpline.Domain.AgentManagement.Aggregates;

namespace FlowSpline.Application.AgentManagement.Repositories;

public interface IAgentTeamRepository
{
    Task<AgentTeam?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(AgentTeam team, CancellationToken cancellationToken = default);
    Task UpdateAsync(AgentTeam team, CancellationToken cancellationToken = default);
}
