using System.ComponentModel.DataAnnotations;
using Spriggan.Core.Attributes;

namespace Spriggan.Core.Transport.Options;

[Option("Core:Transport:RabbitMq")]
public class RabbitMqOptions
{
    [Required]
    [Environment("RABBITMQ_DEFAULT_HOST")]
    public string Host { get; set; } = null!;

    [Required]
    [Environment("RABBITMQ_DEFAULT_USER")]
    public string User { get; set; } = null!;

    [Required]
    [Environment("RABBITMQ_DEFAULT_PASS")]
    public string Password { get; set; } = null!;
}
