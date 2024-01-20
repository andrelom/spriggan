using FastEndpoints;
using Spriggan.Core;

namespace Spriggan.Web;

public static class Program
{
    public static void Main(string[] arguments)
    {
        var builder = CreateHostBuilder(arguments);
        var host = builder.Build();

        ConfigureHost(host);

        host.Run();
    }

    // Please do not change this method accessor (private, etc) as the Entity Framework
    // calls it when we are working with migrations.
    public static WebApplicationBuilder CreateHostBuilder(string[] arguments)
    {
        Development.ReadEnvironmentVariables();

        Dependencies.Initialize("Spriggan");

        var builder = WebApplication.CreateBuilder(arguments);

        ConfigureServices(builder.Services, builder.Configuration);

        return builder;
    }

    #region Private Methods

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        //
        // Libraries

        // DI from "FastEndpoints".

        services.AddFastEndpoints();

        //
        // Core

        services
            .AddCore()
            .AddCoreDataProtection(configuration);
    }

    // Be aware that any change in method call order can result in unexpected behavior.
    private static void ConfigureHost(WebApplication app)
    {
        // Step: 01
        app.UseFastEndpoints();
    }

    #endregion
}
