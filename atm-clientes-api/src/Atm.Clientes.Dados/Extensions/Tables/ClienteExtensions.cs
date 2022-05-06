using Atm.Clientes.Domain;
using Microsoft.EntityFrameworkCore;

namespace Atm.Clientes.Dados.Extensions.Tables
{
    internal static class ClienteExtensions
    {
        internal static void SetupCliente(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>()
                        .HasIndex(c => c.Id);

            modelBuilder.Entity<Cliente>()
                        .Property(c => c.Nome)
                        .HasMaxLength(150)
                        .IsRequired();

            modelBuilder.Entity<Cliente>()
                        .Property(c => c.Cpf)
                        .HasMaxLength(25);

            modelBuilder.Entity<Cliente>()
                        .Property(c => c.Email)
                        .HasMaxLength(60);

            modelBuilder.Entity<Cliente>()
                        .Property(c => c.Telefone)
                        .HasMaxLength(20);

            modelBuilder.Entity<Cliente>()
                        .Property(c => c.Endereco)
                        .HasMaxLength(200);

            modelBuilder.Entity<Cliente>()
                        .Property(c => c.Cep)
                        .HasMaxLength(10);
        }
    }
}
