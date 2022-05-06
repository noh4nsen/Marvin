using Atm.Fornecedor.Dados.Repositories;
using Atm.Fornecedor.Domain;
using Atm.Fornecedor.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Atm.Fornecedor.Dados.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        internal static void SetupRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<Domain.Fornecedor>), typeof(Repository<Domain.Fornecedor>));
            services.AddScoped(typeof(IRepository<Produto>), typeof(Repository<Produto>));
            services.AddScoped(typeof(IRepository<HistoricoProduto>), typeof(Repository<HistoricoProduto>));
        }

        internal static void SetupDbContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<DbContext>(options =>
                options.EnableSensitiveDataLogging()
                       .UseNpgsql(connectionString, b => b.MigrationsAssembly(typeof(DbContext).Assembly.FullName))
            );
        }
    }
}
