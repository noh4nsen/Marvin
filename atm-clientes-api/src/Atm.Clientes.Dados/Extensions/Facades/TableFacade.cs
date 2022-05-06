using Atm.Clientes.Dados.Extensions.Tables;
using Microsoft.EntityFrameworkCore;

namespace Atm.Clientes.Dados.Extensions.Facades
{
    internal static class TableFacade
    {
        internal static void Setuptables(this ModelBuilder modelBuilder)
        {
            modelBuilder.SetupCliente();
            modelBuilder.SetupCarro();
        }
    }
}
