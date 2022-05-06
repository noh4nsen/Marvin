using Atm.Atendimento.Api.Features.Orçamentos.Commands.InserirOrcamentoFeature;
using Atm.Atendimento.Api.Features.Orçamentos.Queries.SelecionarOrcamentoByIdFeature;
using Atm.Atendimento.Api.Helpers;
using Atm.Atendimento.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Atm.Atendimento.Api.Extensions.Entities
{
    public static class ProdutoOrcamentoExtensions
    {
        public static ProdutoOrcamento ToDomain(this InserirProdutoCommand request, ProdutoOrcamento entity)
        {
            return new ProdutoOrcamento()
            {
                Id = Guid.NewGuid(),
                Ativo = true,
                IdExterno = entity.IdExterno,
                Quantidade = request.Quantidade,
                ValorUnitario = request.ValorUnitario,
                Percentual = request.Percentual,
                ValorTotal = request.ValorTotal,
                DataCadastro = DateHelper.GetLocalTime()
            };
        }

        public static IEnumerable<SelecionarProdutoQueryResponse> ToQueryResponse(this ICollection<ProdutoOrcamento> list)
        {
            if (list.Count == 0 && !list.Any())
                return new List<SelecionarProdutoQueryResponse>();

            IList<SelecionarProdutoQueryResponse> produtos = new List<SelecionarProdutoQueryResponse>();
            foreach(var entity in list)
                produtos.Add(entity.ToQueryResponse());
            return produtos;
        }

        public static SelecionarProdutoQueryResponse ToQueryResponse(this ProdutoOrcamento entity)
        {
            return new SelecionarProdutoQueryResponse()
            {
                Id = entity.Id,
                ProdutoId = entity.IdExterno,
                ValorUnitario = entity.ValorUnitario,
                Quantidade = entity.Quantidade,
                Percentual= entity.Percentual,
                ValorTotal= entity.ValorTotal
            };
        }
    }
}
