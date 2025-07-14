using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TransferYape.Persistence.Sql;
using Microsoft.EntityFrameworkCore;
using TransferYape.Application.Repositories;
using TransferYape.Persistence.Sql.Interfaces;
using TransferYape.Domain.Transactions.Repositories;
using TransferYape.Persistence.Sql.Transactions.Repositories;

namespace TransferYape.PersistenceSql.Configuration;
public static class PersistenceSqlExtensions
{
    public static void AddPersistenceSql(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddScoped<ITransactionRepository, TransactionRepository>();

        services.AddDbContext<TransactionDbContext>((sp, options) =>
        {
            options.UseSqlServer(configuration.GetConnectionString("SqlServer"));
        });

        services.AddScoped<ITransactionDbContext>(provider => provider.GetRequiredService<TransactionDbContext>());
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}
