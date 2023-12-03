using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Spriggan.Data.Main;

public static class DependencyInjectionConfiguration
{
    #region For: IServiceCollection

    public static IServiceCollection AddDataMain(this IServiceCollection services, IConfiguration configuration)
    {
        //
        // Libraries

        // DI from "Microsoft.EntityFrameworkCore".
        services.AddDbContext<ApplicationDbContext>(options => AddDbContext(options, configuration));

        //
        // Repositories

        // TODO: Add repositories configurations here.

        return services;
    }

    #endregion

    #region Private Methods

    private static void AddDbContext(DbContextOptionsBuilder options, IConfiguration configuration)
    {
        options.UseNpgsql(configuration.GetConnectionString("Main"));
    }

    #endregion
}
