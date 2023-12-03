using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Spriggan.Module.Main;

public static class DependencyInjectionConfiguration
{
    #region For: IServiceCollection

    public static IServiceCollection AddModuleMain(this IServiceCollection services, IConfiguration configuration)
    {
        return services;
    }

    #endregion
}
