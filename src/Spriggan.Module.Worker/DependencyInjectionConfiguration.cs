using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Spriggan.Module.Worker;

public static class DependencyInjectionConfiguration
{
    #region For: IServiceCollection

    public static IServiceCollection AddModuleIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        return services;
    }

    #endregion

    #region Private Methods: Add

    private static void AddIdentity(IServiceCollection services)
    {
        // Intentionally left empty.
    }

    #endregion
}
