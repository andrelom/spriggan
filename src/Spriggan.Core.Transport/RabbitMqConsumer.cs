using EasyNetQ;
using Microsoft.Extensions.Hosting;

namespace Spriggan.Core.Transport;

public class RabbitMqConsumer : IHostedService
{
    private readonly EasyNetQ.IBus _bus;

    public RabbitMqConsumer()
    {
        _bus = RabbitHutch.CreateBus("host=localhost");
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    #region Private Methods

    private Task HandleRequests()
    {
        throw new NotImplementedException();
    }

    private Task HandleSubscribers()
    {
        throw new NotImplementedException();
    }

    #endregion
}
