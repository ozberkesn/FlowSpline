using FlowSpline.Domain.ToolRuntime.Aggregates;

namespace FlowSpline.Application.ToolRuntime.Repositories;

public interface IToolDefinitionRepository
{
    Task<ToolDefinition?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ToolDefinition?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ToolDefinition>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(ToolDefinition tool, CancellationToken cancellationToken = default);
    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);
}
