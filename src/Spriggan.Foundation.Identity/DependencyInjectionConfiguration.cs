using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Spriggan.Core.Extensions;
using Spriggan.Foundation.Identity.Filters;
using Spriggan.Foundation.Identity.Options;

namespace Spriggan.Foundation.Identity;

public static class DependencyInjectionConfiguration
{
    #region For: IServiceCollection

    public static IServiceCollection AddFoundationIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.Load<JwtOptions>();

        //
        // Libraries

        // DI from "Microsoft.AspNetCore.Authentication.JwtBearer".
        AddAuthentication(services, options);

        return services;
    }

    #endregion

    #region For: IApplicationBuilder

    public static IApplicationBuilder UseFoundationIdentity(this IApplicationBuilder app)
    {
        // To not let Microsoft Identity to override claim names.
        // More at: https://stackoverflow.com/a/61900842/224810
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

        app.UseAuthentication();

        app.UseAuthorization();

        return app;
    }

    #endregion

    #region For: MvcOptions

    public static MvcOptions AddAddFoundationIdentity(this MvcOptions options)
    {
        // Do not change the order.
        options.Filters.Add<UserRolesAttributeActionFilter>();
        options.Filters.Add<UserIdAttributeActionFilter>();

        return options;
    }

    #endregion

    #region Private Methods: Add

    private static void AddAuthentication(IServiceCollection services, JwtOptions options)
    {
        var builder = services.AddAuthentication(authentication =>
        {
            authentication.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            authentication.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            authentication.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        });

        builder.AddJwtBearer(jwt =>
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.IssuerKey));

            // In distributed applications, it should be false.
            jwt.SaveToken = false;

            // Parameters used to validate the identity token.
            jwt.TokenValidationParameters = new()
            {
                ValidateIssuer = true,
                ValidIssuer = options.Issuer,
                ValidateAudience = true,
                ValidAudience = options.Audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key
            };
        });
    }

    #endregion
}
