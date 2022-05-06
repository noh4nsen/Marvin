using Atm.Atendimento.Dto;
using System;
using System.Threading.Tasks;

namespace Atm.Atendimento.Services
{
    public interface IProdutoService
    {
        Task<ProdutoOrcamento> GetProdutoById(Guid id);
        Task<ProdutoOrcamento> PutProduto(ProdutoOrcamento produtoOrcamento);
    }
}
