using MassTransit;
using TransferYape.Infrastructure.Configuration.Options;

namespace TransferYape.Api.Configuration.Extensions;

public static class MassTransitExtensions
{
    public static IServiceCollection AddConfiguredMassTransit(this IServiceCollection services,
        IConfiguration configuration)
    {
        var busOptions = configuration.GetSection(BusConfigurationOptions.BusConfiguration);

        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();

            if (busOptions.GetValue<bool>("RabbitMQEnabled"))
                ConfigureWithRabbitMq(x, busOptions.GetValue<string>("RabbitMQConnectionString"));
            else
                ConfigureWithKafka(x, busOptions.GetValue<string>("KafkaConnectionString"));
        });

        return services;
    }

    private static void ConfigureWithKafka(IBusRegistrationConfigurator x,
        string? kafkaConnectionString)
    {
        x.AddRider(rider =>
        {
            rider.UsingKafka((context, k) =>
            {
                k.Host(kafkaConnectionString);
            });
        });
    }

    private static void ConfigureWithRabbitMq(IBusRegistrationConfigurator x,
        string? rabbitMQConnectionString)
    {
        x.UsingRabbitMq((context, cfg) =>
        {
            cfg.Host(rabbitMQConnectionString);
            cfg.UseMessageRetry(r =>
            {
                r.Interval(3, TimeSpan.FromSeconds(10));
            });
            cfg.ConfigureEndpoints(context);
        });
    }
}