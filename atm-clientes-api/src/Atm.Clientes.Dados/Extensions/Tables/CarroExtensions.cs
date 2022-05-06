using Atm.Clientes.Domain;
using Microsoft.EntityFrameworkCore;

namespace Atm.Clientes.Dados.Extensions.Tables
{
    internal static class CarroExtensions
    {
        internal static void SetupCarro(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Carro>()
                        .HasIndex(c => c.Id);

            modelBuilder.Entity<Carro>()
                        .Property(c => c.Placa)
                        .HasMaxLength(15)
                        .IsRequired();

            modelBuilder.Entity<Carro>()
                        .Property(c => c.Descricao)
                        .HasMaxLength(400);

            modelBuilder.Entity<Carro>()
                        .Property(c => c.Modelo)
                        .HasMaxLength(150);

            modelBuilder.Entity<Carro>()
                        .Property(c => c.Marca)
                        .HasMaxLength(50);
        }
    }
}
