using Atm.Fornecedor.Dados.Extensions;
using Atm.Fornecedor.Dados.Extensions.Facades;
using Atm.Fornecedor.Domain;
using Microsoft.EntityFrameworkCore;

namespace Atm.Fornecedor.Dados
{
    public class DbContext : Microsoft.EntityFrameworkCore.DbContext, IDbContext
    {
        public DbContext(DbContextOptions<DbContext> options) : base(options) { }

        public DbSet<Domain.Fornecedor> Fornecedor { get; set; }
        public DbSet<Produto> Produto { get; set; }

        public DbSet<HistoricoProduto> HistoricoProduto { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.SetupConstraints();
            modelBuilder.Setuptables();
        }
    }
}
