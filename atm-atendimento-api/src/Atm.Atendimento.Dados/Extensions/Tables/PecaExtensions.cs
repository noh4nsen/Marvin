using Atm.Atendimento.Domain;
using Microsoft.EntityFrameworkCore;

namespace Atm.Atendimento.Dados.Extensions.Tables
{
    internal static class PecaExtensions
    {
        internal static void SetupPeca(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Peca>()
                        .HasIndex(p => p.Id);
            modelBuilder.Entity<Peca>()
                        .Property(p => p.CodigoNCM)
                        .HasMaxLength(8);
            modelBuilder.Entity<Peca>()
                        .Property(p => p.Nome)
                        .HasMaxLength(60)
                        .IsRequired();
            modelBuilder.Entity<Peca>()
                        .Property(p => p.Descricao)
                        .HasMaxLength(255);
            modelBuilder.Entity<Peca>()
                        .HasOne(p => p.Orcamento)
                        .WithMany(p => p.Pecas);
        }
    }
}
