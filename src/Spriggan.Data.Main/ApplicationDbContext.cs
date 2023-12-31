using Microsoft.EntityFrameworkCore;
using Spriggan.Core.Transport;
using DbContext = Spriggan.Core.Data.DbContext;

namespace Spriggan.Data.Main;

public class ApplicationDbContext : DbContext
{
    private readonly Type _type = typeof(ApplicationDbContext);

    public ApplicationDbContext(IMediator mediator, DbContextOptions<ApplicationDbContext> options) : base(mediator, options)
    {
        // Intentionally left empty.
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(_type.Assembly);

        ApplyNonEntityConfigurations(builder);

        Seeding.Seed(builder);
    }

    private static void ApplyNonEntityConfigurations(ModelBuilder builder)
    {
        // TODO: Add non entity configurations here.
    }
}
