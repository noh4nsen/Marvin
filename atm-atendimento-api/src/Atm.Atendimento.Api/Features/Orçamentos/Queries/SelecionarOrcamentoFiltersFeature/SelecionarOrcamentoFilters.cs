using Atm.Atendimento.Api.Extensions.Entities;
using Atm.Atendimento.Domain;
using Atm.Atendimento.Domain.Enum;
using Atm.Atendimento.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Atm.Atendimento.Api.Features.Orçamentos.Queries.SelecionarOrcamentoFiltersFeature
{
    public class SelecionarOrcamentoFiltersHandler : IRequestHandler<SelecionarOrcamentoFiltersQuery, IEnumerable<SelecionarOrcamentoFiltersQueryResponse>>
    {
        private IRepository<Orcamento> _repository;
        private readonly IRepository<CustoServico> _repositoryCustoServico;
        private readonly IRepository<Pagamento> _repositoryPagamento;
        private readonly SelecionarOrcamentoFiltersQueryValidator _validator;

        public SelecionarOrcamentoFiltersHandler
            (
                IRepository<Orcamento> repository,
                IRepository<CustoServico> repositoryCustoServico,
                IRepository<Pagamento> repositoryPagamento,
                SelecionarOrcamentoFiltersQueryValidator validator
            )
        {
            _repository = repository;
            _repositoryCustoServico = repositoryCustoServico;
            _repositoryPagamento = repositoryPagamento;
            _validator = validator;
        }

        public async Task<IEnumerable<SelecionarOrcamentoFiltersQueryResponse>> Handle(SelecionarOrcamentoFiltersQuery request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new ArgumentNullException("Erro ao processar requisição.");

            IEnumerable<Orcamento> entities = await GetOrcamentosAsync(request, cancellationToken);

            return entities.ToFiltersQueryResponse();
        }

        private async Task<IEnumerable<Orcamento>> GetOrcamentosAsync(SelecionarOrcamentoFiltersQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<Orcamento> entities = await GetOrcamentosAsync(request);
            foreach (Orcamento entity in entities)
            {
                IEnumerable<CustoServico> custoServicos = await GetCustoServicosAsync(request, entity, cancellationToken);
                Pagamento pagamento = await GetPagamentoAsync(request, entity, cancellationToken);
                entity.CustoServicos = custoServicos.ToList();
                entity.Pagamento = pagamento;
            }
            return entities;
        }

        private async Task<IEnumerable<CustoServico>> GetCustoServicosAsync(SelecionarOrcamentoFiltersQuery request, Orcamento entity, CancellationToken cancellationToken)
        {
            IEnumerable<CustoServico> custoServicos = await _repositoryCustoServico.GetAsync(cs => cs.Orcamento.Id.Equals(entity.Id), cs => cs.Servico);
            foreach (var custoServico in custoServicos)
                await _validator.ValidateDataAsync(request, custoServico, cancellationToken);
            return custoServicos;
        }

        private async Task<Pagamento> GetPagamentoAsync(SelecionarOrcamentoFiltersQuery request, Orcamento entity, CancellationToken cancellationToken)
        {
            Pagamento pagamento = await _repositoryPagamento.GetFirstAsync(p => p.Id.Equals(entity.Pagamento.Id), p => p.ModoPagamento);
            await _validator.ValidateDataAsync(request, pagamento, cancellationToken);
            return pagamento;
        }

        private async Task<IEnumerable<Orcamento>> GetOrcamentosAsync(SelecionarOrcamentoFiltersQuery request)
        {
            IEnumerable<Orcamento> entities = await _repository.GetAsync(Predicate(request),
                                                                           o => o.Cliente,
                                                                           o => o.Carro,
                                                                           o => o.Pagamento);
            return entities;
        }

        private Expression<Func<Orcamento, bool>> Predicate(SelecionarOrcamentoFiltersQuery request)
        {
            Expression<Func<Orcamento, bool>> predicate = PredicateBuilder.True<Orcamento>();

            if (!request.ClienteId.Equals(Guid.Empty))
                predicate = predicate.And(s => s.Cliente.IdExterno.Equals(request.ClienteId));
            if (!request.CarroId.Equals(Guid.Empty))
                predicate = predicate.And(s => s.Carro.IdExterno.Equals(request.CarroId));
            if (!request.Status.Equals(StatusEnum.None))
                predicate = predicate.And(s => s.Status.Equals(request.Status));
            if (request.DiaCadastro is not null)
                predicate = predicate.And(s => s.DataCadastro.Date.Day.Equals(request.DiaCadastro));
            if (request.MesCadastro is not null)
                predicate = predicate.And(s => s.DataCadastro.Date.Month.Equals(request.MesCadastro));
            if (request.AnoCadastro is not null)
                predicate = predicate.And(s => s.DataCadastro.Date.Year.Equals(request.AnoCadastro));
            if (request.DiaAgendamento is not null)
                predicate = predicate.And(s => s.DataAgendamento.Value.Day.Equals(request.DiaAgendamento));
            if (request.MesAgendamento is not null)
                predicate = predicate.And(s => s.DataAgendamento.Value.Month.Equals(request.MesAgendamento));
            if (request.AnoAgendamento is not null)
                predicate = predicate.And(s => s.DataAgendamento.Value.Year.Equals(request.AnoAgendamento));
            return predicate;
        }
    }

    public static class PredicateBuilder
    {
        public static Expression<Func<Orcamento, bool>> True<Orcamento>() { return c => true; }

        public static Expression<Func<Orcamento, bool>> And<Orcamento>(this Expression<Func<Orcamento, bool>> expression1, Expression<Func<Orcamento, bool>> expression2)
        {
            var invokedExpr = Expression.Invoke(expression2, expression1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<Orcamento, bool>>
                            (Expression.And(expression1.Body, invokedExpr), expression1.Parameters);
        }
    }
}
