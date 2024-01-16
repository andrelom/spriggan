using Microsoft.Extensions.Hosting;

namespace Spriggan.Core.Transport;

public class RabbitMqConsumer : IHostedService
{
    private static readonly IEnumerable<Type> Requests = GetRequestTypes();

    private static readonly IEnumerable<Type> Notifications = GetNotificationTypes();

    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    #region Private Methods

    private static IEnumerable<Type> GetRequestTypes()
    {
        var target = typeof(IRequest<>);

        return Dependencies.Domain.Where(type =>
        {
            return type.GetInterfaces().Any(source => source.IsGenericType && source.GetGenericTypeDefinition() == target);
        });
    }

    private static IEnumerable<Type> GetNotificationTypes()
    {
        var target = typeof(INotification);

        return Dependencies.Domain.Where(type =>
        {
            return type.GetInterfaces().Any(source => source.IsGenericType && source.GetGenericTypeDefinition() == target);
        });
    }

    #endregion
}
