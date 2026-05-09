using FlowSpline.Domain.AgentManagement.Aggregates;

namespace FlowSpline.Application.AgentManagement.Repositories;

public interface IAgentRepository
{
    Task<AgentDefinition?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<AgentDefinition>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(AgentDefinition agent, CancellationToken cancellationToken = default);
    Task UpdateAsync(AgentDefinition agent, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
}
