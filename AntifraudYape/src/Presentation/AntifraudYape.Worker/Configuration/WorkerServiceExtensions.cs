using AntifraudYape.PersistenceSql.Configuration;
using AntifraudYape.Application.Configuration;
using AntifraudYape.Worker.Configuration.Extensions;

namespace AntifraudYape.Worker.Configuration;

public static class WorkerServiceExtensions
{
    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApplication();
        services.AddPersistenceSql(configuration);
        services.AddPresentation(configuration);
    }
    private static void AddPresentation(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddConfiguredMassTransit(configuration);
    }
}