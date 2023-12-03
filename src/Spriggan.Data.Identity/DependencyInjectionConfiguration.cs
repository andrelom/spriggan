using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spriggan.Data.Identity.Contracts.Entities;

namespace Spriggan.Data.Identity;

public static class DependencyInjectionConfiguration
{
    #region For: IServiceCollection

    public static IServiceCollection AddDataIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        //
        // Libraries

        // DI from "Microsoft.EntityFrameworkCore".
        services.AddDbContext<ApplicationDbContext>(options => AddDbContext(options, configuration));

        // DI from "Microsoft.AspNetCore.Identity.EntityFrameworkCore".
        services.AddIdentity<User, Role>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }

    #endregion

    #region Private Methods

    private static void AddDbContext(DbContextOptionsBuilder options, IConfiguration configuration)
    {
        options.UseNpgsql(configuration.GetConnectionString("Identity"));
    }

    #endregion
}
