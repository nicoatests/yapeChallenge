using AntifraudYape.Infrastructure.Configuration.Options;
using AntifraudYape.Worker.Transactions.Consumers;
using MassTransit;
using TransferYape.Application.Messages.Transactions;

namespace AntifraudYape.Worker.Configuration.Extensions;

public static class MassTransitExtensions
{
    public static IServiceCollection AddConfiguredMassTransit(this IServiceCollection services,
        IConfiguration configuration)
    {
        var busOptions = configuration.GetSection(BusConfigurationOptions.BusConfiguration);

        services.AddMassTransit(x =>
        {

            x.AddConsumers(typeof(AssemblyReference).Assembly);
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
            rider.AddConsumer<TransactionCreatedThenValidateValue>();

            rider.UsingKafka((context, k) =>
            {
                k.Host(kafkaConnectionString);

                k.TopicEndpoint<TransactionCreated>("transaction-validated", "transfer-group", d =>
                {
                    d.ConfigureConsumer<TransactionCreatedThenValidateValue>(context);
                });
            });
        });
    }

    private static void ConfigureWithRabbitMq(IBusRegistrationConfigurator x,
        string? rabbitMQConnectionString)
    {
        x.UsingRabbitMq((context, cfg) =>
        {
            cfg.Host(rabbitMQConnectionString);
            cfg.ConfigureEndpoints(context);
        });
    }
}