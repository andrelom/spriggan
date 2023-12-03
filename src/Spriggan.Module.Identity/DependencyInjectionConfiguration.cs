using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spriggan.Module.Identity.Providers;

namespace Spriggan.Module.Identity;

public static class DependencyInjectionConfiguration
{
    #region For: IServiceCollection

    public static IServiceCollection AddModuleIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        //
        // Libraries

        // DI from "Microsoft.AspNetCore.Identity".
        AddIdentity(services);

        //
        // Providers

        services
            .AddScoped<ISecurityKeyProvider, SymmetricSecurityKeyProvider>()
            .AddScoped<ISecurityTokenProvider, JwtSecurityTokenProvider>();

        return services;
    }

    #endregion

    #region Private Methods: Add

    private static void AddIdentity(IServiceCollection services)
    {
        // Configure the Identity Options for the identity system.
        services.Configure<IdentityOptions>(options =>
        {
            // Sets the User options.
            options.User.RequireUniqueEmail = true;

            // Sets the SignIn options.
            options.SignIn.RequireConfirmedEmail = false;

            // Sets the Lockout options.
            options.Lockout.AllowedForNewUsers = true;
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);

            // Sets the Password options.
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 8;
            options.Password.RequiredUniqueChars = 1;
        });
    }

    #endregion
}
