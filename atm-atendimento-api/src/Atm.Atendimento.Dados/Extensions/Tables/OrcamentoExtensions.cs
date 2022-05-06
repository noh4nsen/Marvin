using Atm.Atendimento.Domain;
using Atm.Atendimento.Dto;
using Microsoft.EntityFrameworkCore;

namespace Atm.Atendimento.Dados.Extensions.Tables
{
    internal static class OrcamentoExtensions
    {
        internal static void SetupOrcamento(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Orcamento>()
                        .HasIndex(o => o.Id);
            modelBuilder.Entity<Orcamento>()
                        .Property(o => o.Descricao)
                        .HasMaxLength(500);
            modelBuilder.Entity<Orcamento>()
                        .HasMany(o => o.Produtos).WithOne(o => o.Orcamento).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Orcamento>()
                        .HasMany(o => o.CustoServicos).WithOne(o => o.Orcamento).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Orcamento>()
                        .HasMany(o => o.Pecas).WithOne(o => o.Orcamento).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
