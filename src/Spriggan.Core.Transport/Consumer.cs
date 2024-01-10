using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Spriggan.Core.Transport;

public class Consumer : IHostedService
{
    private readonly IEnumerable<object?> _handlers;

    public Consumer(IServiceProvider provider)
    {
        _handlers = provider.GetServices(typeof(IRequestHandler<,>));
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
