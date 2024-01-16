using Microsoft.Extensions.Hosting;

namespace Spriggan.Core.Transport;

public class RabbitMqConsumer : IHostedService
{
    private static readonly IEnumerable<Type> Requests = GetGenericTypes(typeof(IRequest<>));

    private static readonly IEnumerable<Type> Notifications = GetGenericTypes(typeof(INotification));

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await BindRequestHandlers();
        await BindNotificationHandlers();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    #region Private Methods

    private static IEnumerable<Type> GetGenericTypes(Type target)
    {
        return Dependencies.Domain.Where(type =>
        {
            return type.GetInterfaces().Any(source => source.IsGenericType && source.GetGenericTypeDefinition() == target);
        });
    }

    private Task BindRequestHandlers()
    {
        return Task.CompletedTask;
    }

    private Task BindNotificationHandlers()
    {
        return Task.CompletedTask;
    }

    #endregion
}
