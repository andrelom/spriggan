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

        return services;
    }

    #endregion

    #region Private Methods: Add

    private static void AddQuartz(IServiceCollectionQuartzConfigurator configurator)
    {
        // Intentionally left empty.
    }

    #endregion
}
