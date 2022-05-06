using Atm.Atendimento.Dados.Extensions;
using Atm.Atendimento.Dados.Extensions.Facades;
using Atm.Atendimento.Domain;
using Microsoft.EntityFrameworkCore;

namespace Atm.Atendimento.Dados
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
