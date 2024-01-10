using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spriggan.Core.Extensions;
using Spriggan.Core.Transport.Options;

namespace Spriggan.Core.Transport;

public static class DependencyInjectionConfiguration
{
    #region For: IServiceCollection

    public static IServiceCollection AddCoreTransport(this IServiceCollection services, IConfiguration configuration)
    {
        //
        // Libraries

        // DI from "MassTransit".
        AddMassTransit(services, configuration);

        //
        // Services

        services.AddTransient<IBus, Bus>();

        return services;
    }

    #endregion

    #region Private Methods

    private static void AddMassTransit(IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.Load<RabbitMqOptions>();

        // Bus: Local
        services.AddMassTransit<ILocalBus>(configurator =>
        {
            // Consumers.
            configurator.AddConsumers(Dependencies.Assemblies.ToArray());

            // In Memory Transport.
            configurator.UsingInMemory((context, cfg) =>
            {
                cfg.ConfigureEndpoints(context);
            });
        });

        // Bus: Remote
        services.AddMassTransit<IRemoteBus>(configurator =>
        {
            // Consumers.
            configurator.AddConsumers(Dependencies.Assemblies.ToArray());

            // RabbitMQ Transport.
            configurator.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(options.Host, "/", (host) => {
                    host.Username(options.User);
                    host.Password(options.Password);
                });

                cfg.ConfigureEndpoints(context);
            });
        });
    }

    #endregion
}
