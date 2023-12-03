using Spriggan.Core.Transport;

namespace Spriggan.Core.Data.Notifications;

public class DbContextChangesNotification : INotification
{
    public IEnumerable<KeyValuePair<Guid, Type>> Updates { get; set; } = Array.Empty<KeyValuePair<Guid, Type>>();
}
