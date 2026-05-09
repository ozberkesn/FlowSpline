using FlowSpline.Application.ExecutionEngine.Repositories;
using FlowSpline.Domain.ExecutionEngine.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace FlowSpline.Persistence.ExecutionEngine;

internal sealed class ExecutionRunRepository : IExecutionRunRepository
{
    private readonly FlowSplineDbContext _db;

    public ExecutionRunRepository(FlowSplineDbContext db) => _db = db;

    public async Task<ExecutionRun?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _db.ExecutionRuns
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task AddAsync(ExecutionRun run, CancellationToken cancellationToken = default)
    {
        await _db.ExecutionRuns.AddAsync(run, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(ExecutionRun run, CancellationToken cancellationToken = default)
    {
        _db.ExecutionRuns.Update(run);
        await _db.SaveChangesAsync(cancellationToken);
    }
}
