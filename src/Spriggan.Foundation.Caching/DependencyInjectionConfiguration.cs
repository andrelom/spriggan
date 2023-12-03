using Microsoft.Extensions.DependencyInjection;

namespace Spriggan.Foundation.Caching;

public static class DependencyInjectionConfiguration
{
    #region For: IServiceCollection

    public static IServiceCollection AddFoundationCaching(this IServiceCollection services)
    {
        //
        // Services

        services
            .AddScoped<IMemoryCache, MemoryCache>();

        return services;
    }

    #endregion
}
