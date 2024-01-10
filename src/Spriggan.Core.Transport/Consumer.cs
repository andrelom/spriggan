using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Spriggan.Core.Extensions;
using Spriggan.Core.Transport.Options;

namespace Spriggan.Core.Transport;

public class Consumer : IHostedService
{
    private readonly ILogger<Consumer> _logger;

    private readonly RabbitMqOptions _options;

    public Consumer(ILogger<Consumer> logger, IConfiguration configuration)
    {
        _logger = logger;
        _options = configuration.Load<RabbitMqOptions>();
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

    private string ToQueueName(Type type)
    {
        var name = type.FullName;

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new Exception();
        }

        return Regex.Replace(name, "[^a-zA-Z0-9]", string.Empty);
    }

    private IEnumerable<string> GetQueueNames()
    {
        return GetRequestHandlerTypes().Select(ToQueueName);
    }

    private IEnumerable<Type> GetRequestHandlerTypes()
    {
        var target = typeof(IRequestHandler<,>);

        return Dependencies.Domain.Where(type =>
        {
            return type.GetInterfaces().Any(source => source.IsGenericType && source.GetGenericTypeDefinition() == target);
        });
    }

    #endregion
}
