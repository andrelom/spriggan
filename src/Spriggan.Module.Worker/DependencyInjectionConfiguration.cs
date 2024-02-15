using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Spriggan.Module.Worker.Features.Example;

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

    #region Private Methods

    private static void Configure(this IServiceCollectionQuartzConfigurator configurator)
    {
        // Jobs
        configurator.UseExampleJob();

        // Triggers
        configurator.UseExampleTrigger();
    }

    #endregion

    #region Private Methods: Add

    private static void AddQuartz(IServiceCollectionQuartzConfigurator configurator)
    {
        configurator.SchedulerId = "Spriggan Worker";

        configurator.Configure();

        configurator.UseSimpleTypeLoader();

        configurator.UseDefaultThreadPool(options =>
        {
            options.MaxConcurrency = 2;
        });

        configurator.UsePersistentStore(options =>
        {
            options.UseBinarySerializer();

            options.UsePostgres(provider =>
            {
                provider.ConnectionStringName = "Worker";
            });
        });
    }

    private static void AddQuartzHostedService(QuartzHostedServiceOptions options)
    {
        // When shutting down we want jobs to complete gracefully.
        options.WaitForJobsToComplete = true;

        // After server startup, delays before starting triggers.
        options.StartDelay = TimeSpan.FromMinutes(1);
    }

    #endregion
}
