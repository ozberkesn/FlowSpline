using FlowSpline.Domain.ExecutionEngine.Aggregates;

namespace FlowSpline.Application.ExecutionEngine.Repositories;

public interface IExecutionRunRepository
{
    Task<ExecutionRun?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(ExecutionRun run, CancellationToken cancellationToken = default);
    Task UpdateAsync(ExecutionRun run, CancellationToken cancellationToken = default);
}
