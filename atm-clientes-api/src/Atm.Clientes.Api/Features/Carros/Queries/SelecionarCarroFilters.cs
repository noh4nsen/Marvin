using Atm.Clientes.Api.Extensions.Entities;
using Atm.Clientes.Domain;
using Atm.Clientes.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Atm.Clientes.Api.Features.Carros.Queries
{
    public class SelecionarCarroFiltersQuery : IRequest<IEnumerable<SelecionarCarroByIdQueryResponse>>
    {
        public string Placa { get; set; }
        public string Modelo { get; set; }
        public bool? Ativo { get; set; }
    }

    public class SelecionarCarroFiltersQueryHandler : IRequestHandler<SelecionarCarroFiltersQuery, IEnumerable<SelecionarCarroByIdQueryResponse>>    
    {
        private readonly IRepository<Carro> _repository;

        public SelecionarCarroFiltersQueryHandler(IRepository<Carro> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<SelecionarCarroByIdQueryResponse>> Handle(SelecionarCarroFiltersQuery request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new ArgumentNullException("Erro ao processar requisição");

            IEnumerable<Carro> entities = await GetCarrosAsync(request);

            return entities.ToFiltersQueryResponse();
        }

        private async Task<IEnumerable<Carro>> GetCarrosAsync(SelecionarCarroFiltersQuery request)
        {
            IEnumerable<Carro> carros = await _repository.GetAsync(Predicate(request), c => c.Clientes);
            return carros;
        }

        private Expression<Func<Carro, bool>> Predicate(SelecionarCarroFiltersQuery request)
        {
            Expression<Func<Carro, bool>> predicate = PredicateBuilder.True<Carro>();

            if (!request.Placa.Equals(string.Empty))
                predicate = predicate.And(c => c.Placa.ToUpper().Contains(request.Placa.ToUpper()));
            if (!request.Modelo.Equals(string.Empty))
                predicate = predicate.And(c => c.Modelo.ToUpper().Contains(request.Modelo.ToUpper()));
            if (request.Ativo != null)
                predicate = predicate.And(c => c.Ativo.Equals(request.Ativo));
            return predicate;
        }
    }

    public static class PredicateBuilder
    {
        public static Expression<Func<Carro, bool>> True<Carro>() { return c => true; }

        public static Expression<Func<Carro, bool>> And<Carro>(this Expression<Func<Carro, bool>> expression1, Expression<Func<Carro, bool>> expression2)
        {
            var invokedExpr = Expression.Invoke(expression2, expression1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<Carro, bool>>
                            (Expression.And(expression1.Body, invokedExpr), expression1.Parameters);
        }
    }
}
