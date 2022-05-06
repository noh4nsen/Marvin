using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Atm.Atendimento.Dados.Extensions
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
