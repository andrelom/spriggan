namespace Spriggan.Core.Transport;

public interface INotificationHandler<in TNotification>
    where TNotification : INotification
{
    // Intentionally left empty.
}
