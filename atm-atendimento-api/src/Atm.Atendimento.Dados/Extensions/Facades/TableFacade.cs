using Atm.Atendimento.Dados.Extensions.Tables;
using Microsoft.EntityFrameworkCore;

namespace Atm.Atendimento.Dados.Extensions.Facades
{
    internal static class TableFacade
    {
        internal static void Setuptables(this ModelBuilder modelBuilder)
        {
            modelBuilder.SetupOrcamento();
            modelBuilder.SetupServico();
            modelBuilder.SetupPeca();
            modelBuilder.SetupCustoServico();
        }
    }
}
