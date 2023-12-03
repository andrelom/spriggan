using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Spriggan.Core.Data.Extensions;
using Spriggan.Core.Transport;

namespace Spriggan.Core.Data;

public abstract class DbContext : Microsoft.EntityFrameworkCore.DbContext, IUnitOfWork
{
    private readonly IMediator _mediator;

    protected DbContext(IMediator mediator, DbContextOptions options) : base(options)
    {
        _mediator = mediator;
    }

    public async Task<bool> Commit()
    {
        var entries = ChangeTracker.Entries().ToArray();

        Prepare(entries);

        if (await base.SaveChangesAsync() > 0 is var ok)
        {
            await _mediator.NotifyDbContextChanges(entries);
        }

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
