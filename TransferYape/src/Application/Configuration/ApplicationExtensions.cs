using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace TransferYape.Application.Configuration;
public static class ApplicationExtensions
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        var executingAssembly = Assembly.GetExecutingAssembly();
        services.AddValidatorsFromAssembly(executingAssembly);

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(typeof(ApplicationExtensions).Assembly);
        });
        services.AddAutoMapper(executingAssembly);
    }
}
