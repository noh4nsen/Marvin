using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Atm.Fornecedor.Dados.Extensions
{
    public static class ServiceCollectionFacade
    {
        public static void SetupDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.SetupDbContext(configuration.GetConnectionString("DbContext"));
            services.SetupRepositories();
        }
    }
}
