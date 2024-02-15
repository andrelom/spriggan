using Quartz;
using Spriggan.Module.Worker.Jobs;

namespace Spriggan.Module.Worker;

public static class QuartzConfigurator
{
    public static void UseJobs(this IServiceCollectionQuartzConfigurator configurator)
    {
        // Job: Example
        configurator.AddJob<ExampleJob>(job => job
            .WithIdentity(QuartzKeys.Example)
            .StoreDurably()
        );
    }

    public static void UseTriggers(this IServiceCollectionQuartzConfigurator configurator)
    {
        // Trigger: Example
        configurator.AddTrigger(trigger => trigger
            .ForJob(QuartzKeys.Example)
            .StartNow()
            .WithSimpleSchedule(schedule => schedule
                .WithInterval(TimeSpan.FromMinutes(1))
                .RepeatForever()
            )
        );
    }
}
