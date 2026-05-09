using FlowSpline.Domain.AgentManagement.Aggregates;
using FlowSpline.Domain.ExecutionEngine.Aggregates;
using FlowSpline.Domain.ToolRuntime.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace FlowSpline.Persistence;

public sealed class FlowSplineDbContext : DbContext
{
    public FlowSplineDbContext(DbContextOptions<FlowSplineDbContext> options) : base(options) { }

    public DbSet<AgentDefinition> Agents => Set<AgentDefinition>();
    public DbSet<AgentTeam> AgentTeams => Set<AgentTeam>();
    public DbSet<ExecutionRun> ExecutionRuns => Set<ExecutionRun>();
    public DbSet<ToolDefinition> ToolDefinitions => Set<ToolDefinition>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("vector");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FlowSplineDbContext).Assembly);
    }
}
