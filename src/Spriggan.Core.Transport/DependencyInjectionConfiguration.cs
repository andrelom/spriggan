using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spriggan.Core.Transport.Behaviors;

namespace Spriggan.Core.Transport;

public static class DependencyInjectionConfiguration
{
    #region For: IServiceCollection

    public static IServiceCollection AddCoreTransport(this IServiceCollection services, IConfiguration configuration)
    {
        //
        // Libraries

        // DI from "MediatR".
        AddMediatR(services);

        //
        // Services

        services.AddTransient<IMediator, Mediator>();
        services.AddTransient<IRabbitMqClient, RabbitMqClient>();

        //
        // Hosted Services

        services.AddHostedService<RabbitMqConsumer>();

        return services;
    }

    #endregion

    #region Private Methods

    private static void AddMediatR(IServiceCollection services)
    {
        // Local Consumers.
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssemblies(Dependencies.Assemblies.ToArray());
        });

        // Behaviors.
        // Do not change the order.
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(FailFastRequestBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ExceptionBehavior<,>));
    }

    #endregion
}
