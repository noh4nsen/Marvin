using Atm.Atendimento.Api.Features.Orçamentos.Queries.SelecionarOrcamentoByIdFeature;
using Atm.Atendimento.Api.Features.Servicos.Commands;
using Atm.Atendimento.Api.Features.Servicos.Queries;
using Atm.Atendimento.Domain;
using System.Collections.Generic;

namespace Atm.Atendimento.Api.Extensions.Entities
{
    public static class ServicoExtensions
    {
        public static Servico ToDomain(this InserirServicoCommand request)
        {
            return new Servico()
            {
                Nome = request.Nome,
                ValorAtual = request.ValorAtual,
                Ativo = true
            };
        }

        public static InserirServicoCommandResponse ToInsertResponse(this Servico entity)
        {
            return new InserirServicoCommandResponse()
            {
                Id = entity.Id,
                DataCadastro = entity.DataCadastro
            };
        }

        public static void Update(this AtualizarServicoCommand request, Servico entity)
        {
            entity.Nome = request.Nome;
            entity.ValorAtual = request.ValorAtual;
            entity.Ativo = true;
        }

        public static void Update(this CustoServico request, Servico entity)
        {
            entity.ValorAtual = request.Valor;
            entity.CustoServicoAtual = request.Id;
        }

        public static AtualizarServicoCommandResponse ToUpdateResponse(this Servico entity)
        {
            return new AtualizarServicoCommandResponse()
            {
                DataAtualizacao = entity.DataAtualizacao
            };
        }

        public static RemoverServicoCommandResponse ToRemoveResponse(this Servico entity)
        {
            return new RemoverServicoCommandResponse()
            {
                Id = entity.Id
            };
        }

        public static SelecionarServicoByIdQueryResponse ToQueryResponse(this Servico entity)
        {
            return new SelecionarServicoByIdQueryResponse()
            {
                Id = entity.Id,
                Ativo = entity.Ativo,
                Nome = entity.Nome,
                ValorAtual = entity.ValorAtual
            };
        }

        public static IEnumerable<SelecionarServicoByIdQueryResponse> ToFiltersQueryResponse(this IEnumerable<Servico> list)
        {
            IList<SelecionarServicoByIdQueryResponse> response = new List<SelecionarServicoByIdQueryResponse>();
            foreach (Servico entity in list)
                response.Add(entity.ToQueryResponse());
            return response;
        }
    }
}
