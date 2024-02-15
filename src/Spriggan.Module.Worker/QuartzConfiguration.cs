using Quartz;
using Spriggan.Module.Worker.Jobs;

namespace Spriggan.Module.Worker;

public static class QuartzConfiguration
{
    public static void UseJobs(this IServiceCollectionQuartzConfigurator configurator)
    {
        // Job: Example
        configurator.AddJob<ExampleJob>(job => job
            .WithIdentity(ExampleJob.Key)
            .StoreDurably()
        );
    }

    public static void UseTriggers(this IServiceCollectionQuartzConfigurator configurator)
    {
        // Trigger: Example
        configurator.AddTrigger(trigger => trigger
            .ForJob(ExampleJob.Key)
            .StartNow()
            .WithSimpleSchedule(schedule => schedule
                .WithInterval(TimeSpan.FromMinutes(1))
                .RepeatForever()
            )
        );
    }
}
