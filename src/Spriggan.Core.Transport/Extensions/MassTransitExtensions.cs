using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Spriggan.Core.Transport.Extensions;

public static class MassTransitExtensions
{
    public static void AddMassTransitConsumers(this IBusRegistrationConfigurator configurator)
    {
        var massTransitConsumerGenericType = typeof(MassTransitConsumer<,>);
        var consumerGenericType = typeof(IConsumer<,>);
        var consumerTypes = Dependencies.Domain.Where(item => item.GetInterfaces().Any(type => type.IsGenericType && type.GetGenericTypeDefinition() == consumerGenericType));

        foreach (var consumerType in consumerTypes)
        {
            var closedConsumerGenericType = consumerType.GetInterfaces().First(type => type.IsGenericType && type.GetGenericTypeDefinition() == consumerGenericType);

            configurator.AddScoped(closedConsumerGenericType, consumerType);

            var closedMassTransitConsumerGenericType = massTransitConsumerGenericType.MakeGenericType(closedConsumerGenericType.GetGenericArguments());

            configurator.AddConsumer(closedMassTransitConsumerGenericType);
        }
    }
}
