using Microsoft.Extensions.Logging;
using Quartz;

namespace Spriggan.Module.Worker.Jobs;

public class ExampleJob : IJob
{
    private readonly ILogger<ExampleJob> _logger;

    public ExampleJob(ILogger<ExampleJob> logger)
    {
        _logger = logger;
    }

    public Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("The {0} job is executing", nameof(ExampleJob));

        return Task.CompletedTask;
    }
}
