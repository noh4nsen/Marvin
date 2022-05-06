using Atm.Autenticador.Dados.Extensions;
using Atm.Autenticador.Dados.Extensions.Facades;
using Atm.Autenticador.Domain;
using Microsoft.EntityFrameworkCore;

namespace Atm.Autenticador.Dados
{
    public class DbContext : Microsoft.EntityFrameworkCore.DbContext, IDbContext
    {
        public DbContext(DbContextOptions<DbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.SetupConstraints();
            modelBuilder.Setuptables();
        }
    }
}
