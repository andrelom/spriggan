using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Spriggan.Core.Data.Extensions;

namespace Spriggan.Core.Data;

public abstract class DbContext : Microsoft.EntityFrameworkCore.DbContext, IUnitOfWork
{
    protected DbContext(DbContextOptions options) : base(options)
    {
        // Intentionally left empty.
    }

    public async Task<bool> Commit()
    {
        var entries = ChangeTracker.Entries().ToArray();

        Prepare(entries);

        var ok = await base.SaveChangesAsync() > 0;

        return ok;
    }

    public void ChangeState(object model, EntityState state)
    {
        var entry = Entry(model);

        entry.State = state;
    }

    #region Prvate Methods

    private static void Prepare(EntityEntry[] entries)
    {
        var changes = entries.Where(entry => entry.State is EntityState.Added or EntityState.Modified);

        foreach (var change in changes)
        {
            if (change.Entity is not Entity)
            {
                continue;
            }

            change.TrimValues();
            change.AddTimestamps();
        }
    }

    #endregion
}
