using AntifraudYape.Infrastructure.Configuration.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
namespace LLT.SmartVenues.PassesService.Infrastructure.Configuration;

public static class InfrastructureExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        //Register your Infrastructure services
        services.AddSingleton<IValidateOptions<BusConfigurationOptions>, BusConfigurationOptionsValidation>();
    }
}