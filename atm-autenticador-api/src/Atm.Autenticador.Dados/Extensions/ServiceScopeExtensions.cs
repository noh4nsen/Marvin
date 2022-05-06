using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Atm.Autenticador.Dados.Extensions
{
    public static class ServiceScopeExtensions
    {
        public static void DatabaseMigrate(this IServiceScope serviceScope)
        {
            using var context = GetContext(serviceScope);
            context.Database.Migrate();
        }

        private static DbContext GetContext(IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider
                               .GetService<DbContext>();
        }
    }
}
