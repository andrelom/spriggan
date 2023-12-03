using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Spriggan.Core.Data.Extensions;

internal static class EntityEntryExtensions
{
    public static void TrimValues(this EntityEntry entry)
    {
        const string type = "String";

        var properties = entry.Properties.Where(property =>
            property.Metadata.ClrType.Name.Equals(type, StringComparison.OrdinalIgnoreCase));

        foreach (var property in properties)
        {
            if (property.CurrentValue is string value)
            {
                property.CurrentValue = value?.Trim();
            }
        }
    }

    public static void AddTimestamps(this EntityEntry entry)
    {
        if (entry.Entity as Entity is not { } target)
        {
            return;
        }

        var now = DateTime.UtcNow;

        switch (entry.State)
        {
            case EntityState.Added:
                target.CreatedDate = now;
                target.UpdatedDate = now;
                break;
            case EntityState.Modified:
                target.UpdatedDate = now;
                break;
        }
    }
}
