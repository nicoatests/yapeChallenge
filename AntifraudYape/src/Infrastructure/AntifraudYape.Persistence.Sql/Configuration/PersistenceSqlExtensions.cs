using AntifraudYape.Application.Repositories;
using AntifraudYape.Domain.Transactions.Repositories;
using AntifraudYape.Persistence.Sql;
using AntifraudYape.Persistence.Sql.Interfaces;
using AntifraudYape.Persistence.Sql.Transactions.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using AntifraudYape.Application.Transactions.Repositories;

namespace AntifraudYape.PersistenceSql.Configuration;
public static class PersistenceSqlExtensions
{
    public static void AddPersistenceSql(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ITransactionReadModelRepository, TransactionRepository>();

        services.AddDbContext<TransactionDbContext>((sp, options) =>
        {
            options.UseSqlServer(configuration.GetConnectionString("SqlServer"));
        });

        services.AddScoped<ITransactionDbContext>(provider => provider.GetRequiredService<TransactionDbContext>());
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}
