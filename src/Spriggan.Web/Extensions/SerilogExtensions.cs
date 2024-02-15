using Serilog;
using Serilog.Events;

namespace Spriggan.Web.Extensions;

public static class SerilogExtensions
{
    public static void AddSerilog(this IServiceCollection services)
    {
        const string path = "./tmp/logs/{Date}.log";
        const string template = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}";

        Log.Logger = new LoggerConfiguration()
            .WriteTo.RollingFile(path, LogEventLevel.Verbose, template)
            .CreateLogger();

        services.AddLogging(builder => builder
            .ClearProviders()
            .AddSerilog()
        );
    }
}
