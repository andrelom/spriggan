namespace Spriggan.Core.Transport;

public interface INotificationHandler<in TNotification> :
    MediatR.INotificationHandler<TNotification>
    where TNotification : INotification
{
    // Intentionally left empty.
}
