using Atm.Atendimento.Domain;
using Microsoft.EntityFrameworkCore;

namespace Atm.Atendimento.Dados.Extensions.Tables
{
    internal static class CustoServicoExtensions
    {
        internal static void SetupCustoServico(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustoServico>()
                        .HasIndex(cs => cs.Id);
            modelBuilder.Entity<CustoServico>()
                        .Property(s => s.Descricao)
                        .HasMaxLength(255);
        }
    }
}
