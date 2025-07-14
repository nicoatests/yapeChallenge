namespace AntifraudYape.Infrastructure.Configuration.Options;
public class BusConfigurationOptions
{
    public const string BusConfiguration = "BusConfiguration";

    public bool RabbitMQEnabled { get; set; }

    public string? RabbitMQConnectionString { get; set; }

    public string? KafkaConnectionString { get; set; }
}
