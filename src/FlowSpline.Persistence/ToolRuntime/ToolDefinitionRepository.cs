using FlowSpline.Application.ToolRuntime.Repositories;
using FlowSpline.Domain.ToolRuntime.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace FlowSpline.Persistence.ToolRuntime;

internal sealed class ToolDefinitionRepository : IToolDefinitionRepository
{
    private readonly FlowSplineDbContext _db;

    public ToolDefinitionRepository(FlowSplineDbContext db) => _db = db;

    public async Task<ToolDefinition?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _db.ToolDefinitions
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<ToolDefinition?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        => await _db.ToolDefinitions
            .FirstOrDefaultAsync(x => x.Name == name, cancellationToken);

    public async Task<IReadOnlyList<ToolDefinition>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _db.ToolDefinitions
            .ToListAsync(cancellationToken);

    public async Task AddAsync(ToolDefinition tool, CancellationToken cancellationToken = default)
    {
        await _db.ToolDefinitions.AddAsync(tool, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
        => await _db.ToolDefinitions.AnyAsync(x => x.Name == name, cancellationToken);
}
