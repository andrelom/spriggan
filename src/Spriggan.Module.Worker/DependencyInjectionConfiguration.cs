using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Spriggan.Module.Worker.Jobs;

namespace Spriggan.Module.Worker;

public static class DependencyInjectionConfiguration
{
    #region For: IServiceCollection

    public static IServiceCollection AddModuleWorker(this IServiceCollection services, IConfiguration configuration)
    {
        //
        // Libraries

        // DI from "Quartz".
        services.AddQuartz(AddQuartz);
        services.AddQuartzHostedService(AddQuartzHostedService);

        return services;
    }

    #endregion

    #region Private Methods: Add

    private static void AddQuartz(IServiceCollectionQuartzConfigurator configurator)
    {
        configurator.SchedulerId = "Spriggan Worker";

        configurator.UseTriggers();

        configurator.UseSimpleTypeLoader();

        configurator.UseInMemoryStore();

        configurator.UseDefaultThreadPool(options =>
        {
            options.MaxConcurrency = 2;
        });
    }

    private static void AddQuartzHostedService(QuartzHostedServiceOptions options)
    {
        // When shutting down we want jobs to complete gracefully.
        options.WaitForJobsToComplete = true;
    }

    #endregion

    #region Private Methods: Extensions

    private static void UseTriggers(this IServiceCollectionQuartzConfigurator configurator)
    {
        // Trigger: Example
        configurator.AddJob<ExampleJob>(job => job
            .StoreDurably()
            .WithDescription("Example trigger")
        );
    }

    #endregion
}
