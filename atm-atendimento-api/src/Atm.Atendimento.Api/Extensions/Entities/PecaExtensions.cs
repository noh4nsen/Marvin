using Atm.Atendimento.Api.Features.Orçamentos.Commands.InserirOrcamentoFeature;
using Atm.Atendimento.Api.Features.Orçamentos.Queries.SelecionarOrcamentoByIdFeature;
using Atm.Atendimento.Api.Helpers;
using Atm.Atendimento.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Atm.Atendimento.Api.Extensions.Entities
{
    public static class PecaExtensions
    {
        public static Peca ToDomain(this InserirPecaCommand request)
        {
            return new Peca()
            {
                Id = Guid.NewGuid(),
                Ativo = true,
                CodigoNCM = request.CodigoNCM,
                Nome = request.Nome,
                Descricao = request.Descricao,
                ValorUnitarioCompra = request.ValorUnitarioCompra,
                ValorUnitarioVenda = request.ValorUnitarioVenda,
                Quantidade = request.Quantidade,
                Percentual = request.Percentual,
                ValorCobrado = request.ValorCobrado,
                DataCadastro = DateHelper.GetLocalTime()
            };
        }

        public static IEnumerable<SelecionarPecaQueryResponse> ToQueryResponse(this ICollection<Peca> list)
        {
            if (list.Count == 0 && !list.Any())
                return new List<SelecionarPecaQueryResponse>();

            IList<SelecionarPecaQueryResponse> pecas = new List<SelecionarPecaQueryResponse>();
            foreach (var entity in list)
                pecas.Add(entity.ToQueryResponse());
            return pecas;
        }

        public static SelecionarPecaQueryResponse ToQueryResponse(this Peca entity)
        {
            return new SelecionarPecaQueryResponse()
            {
                Id = entity.Id,
                CodigoNCM = entity.CodigoNCM,
                Nome = entity.Nome,
                Descricao = entity.Descricao,
                ValorUnitarioCompra = entity.ValorUnitarioCompra,
                ValorUnitarioVenda = entity.ValorUnitarioVenda,
                Quantidade = entity.Quantidade,
                Percentual = entity.Percentual,
                ValorCobrado = entity.ValorCobrado
            };
        }
    }
}
