using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Spriggan.Core.Transport;

public class ConsumerHostedService : IHostedService
{
    private readonly ILogger<ConsumerHostedService> _logger;

    private readonly IEnumerable<Type> _types = GetRequestHandlerTypes();

    public ConsumerHostedService(ILogger<ConsumerHostedService> logger)
    {
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    #region Private Methods

    private static IEnumerable<Type> GetRequestHandlerTypes()
    {
        var target = typeof(IRequestHandler<,>);
        var types = Dependencies.Domain.SelectMany(type => type.GetInterfaces()).Where(type => type.IsGenericType);

        return types.Where(source => source.GetGenericTypeDefinition() == target);
    }

    #endregion
}
