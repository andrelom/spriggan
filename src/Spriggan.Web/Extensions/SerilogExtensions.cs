using Serilog;
using Serilog.Events;

namespace Spriggan.Web.Extensions;

public static class SerilogExtensions
{
    public static void AddSerilog(this IServiceCollection services)
    {
        const string path = "./tmp/logs/{Date}.txt";
        const string template = "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}";

        Log.Logger = new LoggerConfiguration()
            .WriteTo.RollingFile(path, LogEventLevel.Verbose, template)
            .CreateLogger();

        services.AddLogging(builder => builder
            .ClearProviders()
            .AddSerilog()
        );
    }
}
