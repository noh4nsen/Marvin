using Atm.Fornecedor.Domain;
using Microsoft.EntityFrameworkCore;

namespace Atm.Fornecedor.Dados.Extensions.Tables
{
    internal static class ProdutoExtensions
    {
        internal static void SetupProduto(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Produto>()
                        .Property(p => p.CodigoNCM)
                        .HasMaxLength(8);
            modelBuilder.Entity<Produto>()
                        .Property(p => p.Nome)
                        .HasMaxLength(50)
                        .IsRequired();
            modelBuilder.Entity<Produto>()
                        .Property(p => p.Tipo)
                        .HasMaxLength(50)
                        .IsRequired();
            modelBuilder.Entity<Produto>()
                        .Property(p => p.Descricao)
                        .HasMaxLength(500);
            modelBuilder.Entity<Produto>()
                        .HasOne(p => p.Fornecedor);
            modelBuilder.Entity<Produto>()
                        .HasMany(p => p.HistoricoProduto)
                        .WithOne(p => p.Produto)
                        .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
