using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Spriggan.Core;
using Spriggan.Core.Transport;
using Spriggan.Core.Web;
using Spriggan.Data.Identity;
using Spriggan.Data.Main;
using Spriggan.Foundation.Caching;
using Spriggan.Foundation.Identity;
using Spriggan.Foundation.Mail;
using Spriggan.Module.Identity;
using Spriggan.Module.Main;
using Spriggan.Module.Worker;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Spriggan.Web;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        //
        // Core

        services
            .AddCore()
            .AddCoreDataProtection(_configuration);

        //
        // Core Web

        services
            .AddCoreWeb()
            .AddCoreWebRateLimiter(_configuration)
            .AddCoreWebControllers(AddCoreWebControllers)
            .AddCoreWebSwagger(AddSwagger);

        //
        // Core Transport

        services
            .AddCoreTransport(_configuration);

        //
        // Data

        services
            .AddDataMain(_configuration)
            .AddDataIdentity(_configuration);

        //
        // Foundation

        services
            .AddFoundationCaching()
            .AddFoundationMail()
            .AddFoundationIdentity(_configuration);

        //
        // Modules

        services
            .AddModuleMain(_configuration)
            .AddModuleIdentity(_configuration)
            .AddModuleWorker(_configuration);
    }

    // Be aware that any change in method call order can result in unexpected behavior.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // Step: 01
        app.UseRateLimiter();

        // Step: 02
        app.UseCoreWebRequestLocalization(_configuration);

        // Step: 03
        app.UseCoreWebMiddlewares();

        // Step: 04
        app.UseRouting();

        // Step: 05
        app.UseFoundationIdentity();

        // Step: 06
        app.UseCoreWebEndpoints();

        // Step: 07
        app.UseCoreWebSwagger(UseSwagger);
    }

    #region Private Methods: Add

    private void AddCoreWebControllers(MvcOptions options)
    {
        options.AddAddFoundationIdentity();
    }

    private void AddSwagger(SwaggerGenOptions options)
    {
        options.SwaggerDoc("v1", new OpenApiInfo { Title = "Spriggan.Web", Version = "v1" });
    }

    #endregion

    #region Private Methods: Use

    private void UseSwagger(SwaggerUIOptions options)
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Spriggan.Web v1");
    }

    #endregion
}
