using Atm.Autenticador.Dados.Extensions.Tables;
using Microsoft.EntityFrameworkCore;

namespace Atm.Autenticador.Dados.Extensions.Facades
{
    internal static class TableFacade
    {
        internal static void Setuptables(this ModelBuilder modelBuilder)
        {
            modelBuilder.SetupUsuario();
        }
    }
}
