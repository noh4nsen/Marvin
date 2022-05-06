using Atm.Atendimento.Api.Features.Orçamentos.Commands.AtendimentoFeature;
using Atm.Atendimento.Api.Features.Orçamentos.Commands.InserirOrcamentoFeature;
using Atm.Atendimento.Api.Features.Orçamentos.Commands.RemoverOrcamentoFeature;
using Atm.Atendimento.Api.Features.Orçamentos.Queries.SelecionarOrcamentoByIdFeature;
using Atm.Atendimento.Api.Features.Orçamentos.Queries.SelecionarOrcamentoFiltersFeature;
using Atm.Atendimento.Api.Helpers;
using Atm.Atendimento.Domain;
using Atm.Atendimento.Domain.Enum;
using Atm.Atendimento.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Atm.Atendimento.Api.Extensions.Entities
{
    public static class OrcamentoExtensions
    {
        public static Orcamento ToDomain
            (
                this InserirOrcamentoCommand request,
                ClienteOrcamento cliente,
                CarroOrcamento carro,
                IEnumerable<ProdutoOrcamento> produtos,
                IEnumerable<Peca> pecas,
                IEnumerable<CustoServico> custoServicos,
                Pagamento pagamento
            )
        {
            return new Orcamento()
            {
                Ativo = true,
                Cliente = cliente,
                Carro = carro,
                Produtos = produtos.ToList(),
                Pecas = pecas.ToList(),
                CustoServicos = custoServicos.ToList(),
                Descricao = request.Descricao,
                Pagamento = pagamento,
                Status = StatusEnum.Cadastrado
            };
        }

        public static void ToAgendamento(this AgendarAtendimentoCommand request, Orcamento entity)
        {
            entity.DataAgendamento = request.DataAgendamento;
            entity.DataHoraInicio = request.DataAgendamento;
            entity.Status = StatusEnum.Agendado;
        }

        public static void ToCancelarAgendamento(this Orcamento entity)
        {
            entity.DataAgendamento = null;
            entity.DataHoraInicio = null;
            entity.Status = StatusEnum.Cadastrado;
        }

        public static void ToFinalizarAtendimento(this Orcamento entity, FinalizarAtendimentoCommand request)
        {
            entity.DataHoraFim = request.DataHoraFim ?? DateHelper.GetLocalTime();
            entity.Duracao = ((DateTime)entity.DataHoraFim - (DateTime)entity.DataHoraInicio).TotalHours;
            entity.Status = StatusEnum.Finalizado;
        }

        public static void ToDesfinalizarAtendimento(this Orcamento entity)
        {
            entity.DataHoraFim = null;
            entity.Duracao = null;
            entity.Status = StatusEnum.Agendado;
        }

        public static InserirOrcamentoCommandResponse ToInsertResponse(this Orcamento entity)
        {
            return new InserirOrcamentoCommandResponse()
            {
                Id = entity.Id,
                Datacadastro = entity.DataCadastro
            };
        }

        public static SelecionarOrcamentoByIdQueryResponse ToQueryResponse(this Orcamento entity)
        {
            return new SelecionarOrcamentoByIdQueryResponse()
            {
                Id = entity.Id,
                ClienteId = entity.Cliente.IdExterno,
                CarroId = entity.Carro.IdExterno,
                Descricao = entity.Descricao,
                Status = entity.Status,
                Produtos = entity.Produtos.ToQueryResponse().ToList(),
                Pecas = entity.Pecas.ToQueryResponse().ToList(),
                Servicos = entity.CustoServicos.ToQueryResponse().ToList(),
                Pagamento = entity.Pagamento.ToQueryResponse(),
                DataCadastro = entity.DataCadastro,
                DataAgendamento = entity.DataAgendamento,
                DataHoraInicio = entity.DataHoraInicio,
                DataHoraFim = entity.DataHoraFim,
                Duracao = entity.Duracao,
                ValorFinal = entity.Pagamento.ToQueryResponse().ValorFinal
            };
        }

        public static SelecionarOrcamentoFiltersQueryResponse ToFiltersQueryResponse(this Orcamento entity)
        {
            return new SelecionarOrcamentoFiltersQueryResponse()
            {
                Id = entity.Id,
                ClienteId = entity.Cliente.IdExterno,
                CarroId = entity.Carro.IdExterno,
                ValorFinal = entity.Pagamento.ToQueryResponse().ValorFinal,
                Status = entity.Status,
                DataCadastro = entity.DataCadastro,
                DataAgendamento = entity.DataAgendamento
            };
        }

        public static IEnumerable<SelecionarOrcamentoFiltersQueryResponse> ToFiltersQueryResponse(this IEnumerable<Orcamento> list)
        {
            if (!list.Any())
                return new List<SelecionarOrcamentoFiltersQueryResponse>();

            IList<SelecionarOrcamentoFiltersQueryResponse> response = new List<SelecionarOrcamentoFiltersQueryResponse>();
            foreach (Orcamento entity in list)
                response.Add(entity.ToFiltersQueryResponse());
            return response;
        }

        public static RemoverOrcamentoCommandResponse ToRemoveResponse(this Orcamento entity)
        {
            return new RemoverOrcamentoCommandResponse() { Id = entity.Id };
        }

        public static AgendarAtendimentoCommandResponse ToAgendarResponse(this Orcamento entity)
        {
            return new AgendarAtendimentoCommandResponse()
            {
                Id = entity.Id,
                DataAgendamento = entity.DataAgendamento
            };
        }

        public static CancelarAgendamentoCommandResponse ToCancelarAgendamentoResponse(this Orcamento entity)
        {
            return new CancelarAgendamentoCommandResponse() { Id = entity.Id };
        }

        public static FinalizarAtendimentoCommandResponse ToFinalizarAtendimentoResponse(this Orcamento entity)
        {
            return new FinalizarAtendimentoCommandResponse()
            {
                Id = entity.Id,
                DataHoraFim = entity.DataHoraFim,
                Duracao = entity.Duracao
            };
        }

        public static DesfinalizarAtendimentoCommandResponse ToDesfinalizarAtendimentoResponse(this Orcamento entity)
        {
            return new DesfinalizarAtendimentoCommandResponse()
            {
                Id = entity.Id
            };
        }
    }
}
