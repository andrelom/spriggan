using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

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
        configurator.SchedulerId = "Worker";

        configurator.UseJobs();

        configurator.UseTriggers();

        configurator.UseSimpleTypeLoader();

        configurator.UseDefaultThreadPool(options =>
        {
            options.MaxConcurrency = 2;
        });

        configurator.UsePersistentStore(options =>
        {
            options.UseNewtonsoftJsonSerializer();

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
