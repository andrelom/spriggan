using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Spriggan.Core.Extensions;
using Spriggan.Core.Transport.Options;

namespace Spriggan.Core.Transport;

public class Consumer : IHostedService
{
    private readonly ILogger<Consumer> _logger;

    private readonly RabbitMqOptions _options;

    private readonly IConnection _connection;

    private readonly IModel _channel;

    private readonly IEnumerable<string> _names = GetQueueNames();

    public Consumer(ILogger<Consumer> logger, IConfiguration configuration)
    {
        _logger = logger;
        _options = configuration.Load<RabbitMqOptions>();

        var connection = CreateConnection();
        var channel = connection.CreateModel();

        _connection = connection;
        _channel = channel;

        DeclareQueues("request");
        DeclareQueues("response");
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += (_, deliver) =>
        {
            var body = deliver.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            _logger.LogInformation($"Received message: {message}");

            var reply = _channel.CreateBasicProperties();

            reply.CorrelationId = deliver.BasicProperties.CorrelationId;

            var bytes = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(
                exchange: "",
                routingKey: deliver.BasicProperties.ReplyTo,
                basicProperties: reply,
                body: bytes);
        };

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _channel.Close();
        _connection.Close();

        return Task.CompletedTask;
    }

    #region Private Methods

    private static string ToQueueName(Type type)
    {
        var name = type.FullName;

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new Exception();
        }

        return Regex.Replace(name, "[^a-zA-Z0-9]", string.Empty);
    }

    private static IEnumerable<string> GetQueueNames()
    {
        return GetRequestHandlerTypes().Select(ToQueueName);
    }

    private static IEnumerable<Type> GetRequestHandlerTypes()
    {
        var target = typeof(IRequestHandler<,>);

        return Dependencies.Domain.Where(type =>
        {
            return type.GetInterfaces().Any(source => source.IsGenericType && source.GetGenericTypeDefinition() == target);
        });
    }

    private IConnection CreateConnection()
    {
        var factory = new ConnectionFactory
        {
            HostName = _options.Host,
            UserName = _options.User,
            Password = _options.Password,
        };

        return factory.CreateConnection();
    }

    private void DeclareQueues(string prefix)
    {
        foreach (var name in _names)
        {
            _channel.QueueDeclare(
                queue: $"{prefix}#{name}",
                durable: false,
                exclusive: false,
                autoDelete: false);
        }
    }

    #endregion
}
