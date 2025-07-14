using TransferYape.Api.Configuration.Extensions;
using TransferYape.Application.Configuration;
using TransferYape.PersistenceSql.Configuration;

namespace TransferYape.Api.Configuration;

public static class PresentationExtensions
{
    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPresentationServices(configuration);
        services.AddPersistenceSql(configuration);
        services.AddApplication(configuration);
        services.AddConfiguredMassTransit(configuration);
    }

    private static void AddPresentationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }
}
