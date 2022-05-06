using Atm.Atendimento.Api.Extensions.Entities;
using Atm.Atendimento.Domain;
using Atm.Atendimento.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Atm.Atendimento.Api.Features.Servicos.Queries
{
    public class SelecionarServicoFiltersQuery : IRequest<IEnumerable<SelecionarServicoByIdQueryResponse>>
    {
        public string Nome { get; set; }
    }

    public class SelecionarServicoFiltersQueryHandler : IRequestHandler<SelecionarServicoFiltersQuery, IEnumerable<SelecionarServicoByIdQueryResponse>>
    {
        private readonly IRepository<Servico> _repository;

        public SelecionarServicoFiltersQueryHandler(IRepository<Servico> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<SelecionarServicoByIdQueryResponse>> Handle(SelecionarServicoFiltersQuery request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new ArgumentNullException("Erro ao processar requisição.");

            IEnumerable<Servico> entities = await GetServicosAsync(request);

            return entities.ToFiltersQueryResponse();
        }

        private async Task<IEnumerable<Servico>> GetServicosAsync(SelecionarServicoFiltersQuery request)
        {
            IEnumerable<Servico> entities = await _repository.GetAsync(Predicate(request));
            return entities;
        }

        private Expression<Func<Servico, bool>> Predicate(SelecionarServicoFiltersQuery request)
        {
            Expression<Func<Servico, bool>> predicate = PredicateBuilder.True<Servico>();

            if (!request.Nome.Equals(string.Empty))
                predicate = predicate.And(s => s.Nome.ToUpper().Contains(request.Nome.ToUpper()));
            return predicate;
        }
    }

    public static class PredicateBuilder
    {
        public static Expression<Func<Servico, bool>> True<Servico>() { return c => true; }

        public static Expression<Func<Servico, bool>> And<Servico>(this Expression<Func<Servico, bool>> expression1, Expression<Func<Servico, bool>> expression2)
        {
            var invokedExpr = Expression.Invoke(expression2, expression1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<Servico, bool>>
                            (Expression.And(expression1.Body, invokedExpr), expression1.Parameters);
        }
    }
}
