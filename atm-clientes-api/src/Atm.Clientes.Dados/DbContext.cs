using Atm.Clientes.Dados.Extensions;
using Atm.Clientes.Dados.Extensions.Facades;
using Atm.Clientes.Domain;
using Microsoft.EntityFrameworkCore;

namespace Atm.Clientes.Dados
{
    public class DbContext : Microsoft.EntityFrameworkCore.DbContext, IDbContext
    {
        public DbContext(DbContextOptions<DbContext> options) : base(options) { }

        //public DbSet<Domain.Fornecedor> Fornecedor { get; set; }
        //public DbSet<Produto> Produto { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.SetupConstraints();
            modelBuilder.Setuptables();
        }
    }
}
