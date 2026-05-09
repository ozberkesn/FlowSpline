using FlowSpline.Application.AgentManagement.Repositories;
using FlowSpline.Domain.AgentManagement.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace FlowSpline.Persistence.AgentManagement;

internal sealed class AgentTeamRepository : IAgentTeamRepository
{
    private readonly FlowSplineDbContext _db;

    public AgentTeamRepository(FlowSplineDbContext db) => _db = db;

    public async Task<AgentTeam?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _db.AgentTeams
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task AddAsync(AgentTeam team, CancellationToken cancellationToken = default)
    {
        await _db.AgentTeams.AddAsync(team, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(AgentTeam team, CancellationToken cancellationToken = default)
    {
        _db.AgentTeams.Update(team);
        await _db.SaveChangesAsync(cancellationToken);
    }
}
