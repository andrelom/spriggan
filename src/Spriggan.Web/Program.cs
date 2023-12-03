using Microsoft.AspNetCore.Server.Kestrel.Core;
using Spriggan.Core;

namespace Spriggan.Web;

public static class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    // Please do not change this method accessor (private, etc) as the Entity Framework
    // calls it when we are working with migrations.
    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        Development.ReadEnvironmentVariables();

        Dependencies.Initialize("Spriggan");

        return Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(ConfigureWebHostDefaults);
    }

    #region Private Methods

    private static void ConfigureWebHostDefaults(IWebHostBuilder builder)
    {
        builder.ConfigureKestrel(ConfigureKestrel);

        builder.UseStartup<Startup>();
    }

    private static void ConfigureKestrel(KestrelServerOptions options)
    {
        options.AddServerHeader = false;
    }

    #endregion
}
