using FlowSpline.Application.AgentManagement.Repositories;
using FlowSpline.Domain.AgentManagement.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace FlowSpline.Persistence.AgentManagement;

internal sealed class AgentRepository : IAgentRepository
{
    private readonly FlowSplineDbContext _db;

    public AgentRepository(FlowSplineDbContext db) => _db = db;

    public async Task<AgentDefinition?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _db.Agents
            .Include(x => x.Tools)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<IReadOnlyList<AgentDefinition>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _db.Agents
            .Include(x => x.Tools)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(AgentDefinition agent, CancellationToken cancellationToken = default)
    {
        await _db.Agents.AddAsync(agent, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(AgentDefinition agent, CancellationToken cancellationToken = default)
    {
        _db.Agents.Update(agent);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var agent = await _db.Agents.FindAsync([id], cancellationToken);
        if (agent is not null)
        {
            _db.Agents.Remove(agent);
            await _db.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
        => await _db.Agents.AnyAsync(x => x.Id == id, cancellationToken);
}
