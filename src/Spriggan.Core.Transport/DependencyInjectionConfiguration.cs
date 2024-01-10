using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Spriggan.Core.Transport;

public static class DependencyInjectionConfiguration
{
    #region For: IServiceCollection

    public static IServiceCollection AddCoreTransport(this IServiceCollection services, IConfiguration configuration)
    {
        //
        // Libraries

        // DI from "MassTransit".
        AddMassTransit(services);

        //
        // Services

        services.AddTransient<IBus, Bus>();

        return services;
    }

    #endregion

    #region Private Methods

    private static void AddMassTransit(IServiceCollection services)
    {
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

            // RabbitMq Transport.
            configurator.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("localhost", "/", (host) => {
                    host.Username("guest");
                    host.Password("guest");
                });

                cfg.ConfigureEndpoints(context);
            });
        });
    }

    #endregion
}
