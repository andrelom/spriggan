using Quartz;

namespace Spriggan.Module.Worker.Features.Example;

public static class ExampleExtensions
{
    public static void UseExampleJob(this IServiceCollectionQuartzConfigurator configurator)
    {
        configurator.AddJob<ExampleJob>(job => job
            .WithIdentity(ExampleJob.Key)
            .StoreDurably()
        );
    }

    public static void UseExampleTrigger(this IServiceCollectionQuartzConfigurator configurator)
    {
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
