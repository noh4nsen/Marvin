using Microsoft.EntityFrameworkCore;

namespace Atm.Fornecedor.Dados.Extensions.Tables
{
    internal static class FornecedorExtensions
    {
        internal static void SetupFornecedor(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Domain.Fornecedor>()
                        .Property(p => p.Nome)
                        .HasMaxLength(50)
                        .IsRequired();
            modelBuilder.Entity<Domain.Fornecedor>()
                        .Property(p => p.Cnpj)
                        .HasMaxLength(14);
            modelBuilder.Entity<Domain.Fornecedor>()
                        .Property(p => p.Telefone)
                        .HasMaxLength(11);
            modelBuilder.Entity<Domain.Fornecedor>()
                        .Property(p => p.Email)
                        .HasMaxLength(50);
            modelBuilder.Entity<Domain.Fornecedor>()
                        .Property(p => p.Tipo)
                        .HasMaxLength(10);
            modelBuilder.Entity<Domain.Fornecedor>()
                        .Property(p => p.Endereco)
                        .HasMaxLength(150);
        }
    }
}
