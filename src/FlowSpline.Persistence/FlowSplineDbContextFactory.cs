using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FlowSpline.Persistence;

internal sealed class FlowSplineDbContextFactory : IDesignTimeDbContextFactory<FlowSplineDbContext>
{
    public FlowSplineDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<FlowSplineDbContext>()
            .UseNpgsql("Host=localhost;Port=5432;Database=flowspline;Username=flowspline;Password=flowspline")
            .Options;

        return new FlowSplineDbContext(options);
    }
}
