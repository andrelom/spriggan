using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Spriggan.Core.Data.Notifications;
using Spriggan.Core.Transport;

namespace Spriggan.Core.Data.Extensions;

internal static class MediatorExtensions
{
    public static async Task NotifyDbContextChanges(this IMediator mediator, EntityEntry[] entries)
    {
        var updates = new List<KeyValuePair<Guid, Type>>();
        var changes = entries.Where(entry => entry.State is not EntityState.Unchanged and EntityState.Detached);

        foreach (var change in changes)
        {
            if (change.Entity is Entity { } entity)
            {
                updates.Add(new KeyValuePair<Guid, Type>(entity.Id, entity.GetType()));
            }
        }

        if (!updates.Any())
        {
            return;
        }

        await mediator.Publish(new DbContextChangesNotification
        {
            Updates = updates
        });
    }
}
