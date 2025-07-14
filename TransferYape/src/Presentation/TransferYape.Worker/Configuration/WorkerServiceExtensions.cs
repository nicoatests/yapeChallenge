using TransferYape.PersistenceSql.Configuration;
using TransferYape.Application.Configuration;
using TransferYape.Infrastructure.Configuration;
using TransferYape.Worker.Configuration.Extensions;

namespace TransferYape.Worker.Configuration;

public static class WorkerServiceExtensions
{
    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddInfrastructure(configuration);
        services.AddPresentation(configuration);
        services.AddApplication(configuration);
        services.AddPersistenceSql(configuration);
    }
    private static void AddPresentation(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddConfiguredMassTransit(configuration);
    }
}