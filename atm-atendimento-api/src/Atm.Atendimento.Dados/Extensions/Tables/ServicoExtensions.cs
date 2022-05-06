using Atm.Atendimento.Domain;
using Microsoft.EntityFrameworkCore;

namespace Atm.Atendimento.Dados.Extensions.Tables
{
    internal static class ServicoExtensions
    {
        internal static void SetupServico(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Servico>()
                        .HasIndex(s => s.Id);
            modelBuilder.Entity<Servico>()
                        .Property(s => s.Nome)
                        .HasMaxLength(100)
                        .IsRequired();
        }
    }
}
