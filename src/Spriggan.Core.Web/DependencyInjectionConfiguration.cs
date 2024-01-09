using System.Globalization;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spriggan.Core.Extensions;
using Spriggan.Core.Web.Extensions;
using Spriggan.Core.Web.Factories;
using Spriggan.Core.Web.Filters;
using Spriggan.Core.Web.Middlewares;
using Spriggan.Core.Web.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using RateLimiterOptions = Spriggan.Core.Web.Options.RateLimiterOptions;

namespace Spriggan.Core.Web;

public static class DependencyInjectionConfiguration
{
    #region For: IServiceCollection

    public static IServiceCollection AddCoreWeb(this IServiceCollection services)
    {
        //
        // Libraries

        // DI from "Microsoft.Extensions.Diagnostics.HealthChecks".
        services.AddHealthChecks();

        //
        // Factories

        services
            .AddTransient<IClientErrorFactory, ClientErrorFactory>();

        return services;
    }

    public static IServiceCollection AddCoreWebRateLimiter(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.Load<RateLimiterOptions>();

        services.AddRateLimiter(opt => opt.AddFixedWindowLimiter("fixed", limiter =>
        {
            limiter.Window = TimeSpan.FromSeconds(options.Window);
            limiter.PermitLimit = options.PermitLimit;
            limiter.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            limiter.QueueLimit = options.QueueLimit;
        }));

        return services;
    }

    public static IServiceCollection AddCoreWebControllers(this IServiceCollection services, Action<MvcOptions> setup = null!)
    {
        var builder = services.AddControllers(options =>
        {
            // Do not change the order.
            options.Filters.Add<FromRouteBodyAttributeActionFilter>();
            options.Filters.Add<FailFastActionFilter>();

            setup?.Invoke(options);
        });

        builder.AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

        builder.ConfigureApiBehaviorOptions(options => { options.SuppressModelStateInvalidFilter = true; });

        return services;
    }

    public static IServiceCollection AddCoreWebSwagger(this IServiceCollection services, Action<SwaggerGenOptions> setup = null!)
    {
        services.AddSwaggerGen(options =>
        {
            options.LowercaseDocuments();

            options.AspNetCoreVersioning();

            options.AddSecurity();

            setup?.Invoke(options);
        });

        return services;
    }

    #endregion

    #region For: IApplicationBuilder

    public static IApplicationBuilder UseCoreWebRequestLocalization(this IApplicationBuilder app, IConfiguration configuration)
    {
        var options = configuration.Load<CultureOptions>();
        var cultures = options.Supported?.Select(name => new CultureInfo(name)).ToArray();

        app.UseRequestLocalization(new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture(options.Default),
            SupportedCultures = cultures,
            SupportedUICultures = cultures
        });

        return app;
    }

    public static IApplicationBuilder UseCoreWebMiddlewares(this IApplicationBuilder app)
    {
        // Do not change the order.
        app.UseMiddleware<ExceptionMiddleware>();

        return app;
    }

    public static IApplicationBuilder UseCoreWebEndpoints(this IApplicationBuilder app, Action<IEndpointRouteBuilder> setup = null!)
    {
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGet("-/linkertime", () => Development.LinkerTime);

            endpoints.MapHealthChecks("-/healthz");

            endpoints.MapControllers();

            setup?.Invoke(endpoints);
        });

        return app;
    }

    public static IApplicationBuilder UseCoreWebSwagger(this IApplicationBuilder app, Action<SwaggerUIOptions> setup = null!)
    {
        app.UseSwagger();

        app.UseSwaggerUI(options =>
        {
            options.RoutePrefix = "-/swagger";

            options.DefaultModelsExpandDepth(-1);

            setup?.Invoke(options);
        });

        return app;
    }

    #endregion
}
