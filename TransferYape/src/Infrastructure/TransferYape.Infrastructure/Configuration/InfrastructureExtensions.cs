using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TransferYape.Infrastructure.Configuration.Options;
namespace TransferYape.Infrastructure.Configuration;

public static class InfrastructureExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        //Register your Infrastructure services
        services.AddSingleton<IValidateOptions<BusConfigurationOptions>, BusConfigurationOptionsValidation>();
    }
}