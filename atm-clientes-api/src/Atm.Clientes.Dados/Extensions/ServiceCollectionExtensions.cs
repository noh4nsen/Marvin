using Atm.Clientes.Dados.Repositories;
using Atm.Clientes.Domain;
using Atm.Clientes.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Atm.Clientes.Dados.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        internal static void SetupRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<Carro>), typeof(Repository<Carro>));
            services.AddScoped(typeof(IRepository<Cliente>), typeof(Repository<Cliente>));
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
