using Atm.Fornecedor.Dados.Extensions.Tables;
using Microsoft.EntityFrameworkCore;

namespace Atm.Fornecedor.Dados.Extensions.Facades
{
    internal static class TableFacade
    {
        internal static void Setuptables(this ModelBuilder modelBuilder)
        {
            modelBuilder.SetupProduto();
            modelBuilder.SetupFornecedor();
        }
    }
}
