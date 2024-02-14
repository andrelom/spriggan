using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Spriggan.Module.Worker;

public static class DependencyInjectionConfiguration
{
    #region For: IServiceCollection

    public static IServiceCollection AddModuleWorker(this IServiceCollection services, IConfiguration configuration)
    {
        return services;
    }

    #endregion

    #region Private Methods: Add

    private static void AddWorker(IServiceCollection services)
    {
        // Intentionally left empty.
    }

    #endregion
}
