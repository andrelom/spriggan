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
}
