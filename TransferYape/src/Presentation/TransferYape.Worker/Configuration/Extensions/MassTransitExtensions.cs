using AntifraudYape.Application.Messages.Transactions;
using MassTransit;
using TransferYape.Infrastructure.Configuration.Options;
using TransferYape.Worker.Transactions.Consumers;

namespace TransferYape.Worker.Configuration.Extensions;

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
            rider.AddConsumer<TransactionValidatedThenUpdateStatus>();

            rider.UsingKafka((context, k) =>
            {
                k.Host(kafkaConnectionString);

                k.TopicEndpoint<TransactionValidated>("transaction-validated", "transfer-group", d =>
                {
                    d.ConfigureConsumer<TransactionValidatedThenUpdateStatus>(context);
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