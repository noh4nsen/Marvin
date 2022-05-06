using Atm.Atendimento.Api.Features.Orçamentos.Commands.InserirOrcamentoFeature;
using Atm.Atendimento.Api.Features.Orçamentos.Queries.SelecionarOrcamentoByIdFeature;
using Atm.Atendimento.Api.Helpers;
using Atm.Atendimento.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Atm.Atendimento.Api.Extensions.Entities
{
    public static class CustoServicoExtensions
    {
        public static CustoServico ToDomain(this InserirCustoServicoCommand request, Servico servico)
        {
            return new CustoServico()
            {
                Id = Guid.NewGuid(),
                Ativo = true,
                Servico = servico,
                Valor = request.Valor,
                Descricao = request.Descricao,
                DataCadastro = DateHelper.GetLocalTime()
            };
        }

        public static IEnumerable<SelecionarCustoServicoQueryResponse> ToQueryResponse(this ICollection<CustoServico> list)
        {
            if (list.Count == 0 && !list.Any())
                return new List<SelecionarCustoServicoQueryResponse>();

            List<SelecionarCustoServicoQueryResponse> custoServicos = new List<SelecionarCustoServicoQueryResponse>();
            foreach (var entity in list)
                custoServicos.Add(entity.ToQueryResponse());
            return custoServicos;
        }

        public static SelecionarCustoServicoQueryResponse ToQueryResponse(this CustoServico entity)
        {
            return new SelecionarCustoServicoQueryResponse()
            {
                Id = entity.Id,
                ServicoId = entity.Servico.Id,
                Descricao = entity.Descricao,
                Valor = entity.Valor
            };
        }
    }
}
