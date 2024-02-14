using Quartz;

namespace Spriggan.Module.Worker.Jobs;

public class ExampleJob : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        await Console.Out.WriteLineAsync("The 'Example' job is executing.");
    }
}
