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

namespace Atm.Clientes.Api.Features.Clientes.Queries
{
    public class SelecionarClienteFiltersQuery : IRequest<IEnumerable<SelecionarClienteByIdQueryResponse>>
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Cpf { get; set; }
        public string Telefone { get; set; }
        public bool? Ativo { get; set; }
    }

    public class SelecionarClienteFiltersQueryHandler : IRequestHandler<SelecionarClienteFiltersQuery, IEnumerable<SelecionarClienteByIdQueryResponse>>
    {
        private readonly IRepository<Cliente> _repository;

        public SelecionarClienteFiltersQueryHandler(IRepository<Cliente> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<SelecionarClienteByIdQueryResponse>> Handle(SelecionarClienteFiltersQuery request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new ArgumentNullException("Erro ao processar requisição");

            IEnumerable<Cliente> entities = await GetClientesAsync(request);

            return entities.ToFiltersQueryResponse();
        }

        private async Task<IEnumerable<Cliente>> GetClientesAsync(SelecionarClienteFiltersQuery request)
        {
            IEnumerable<Cliente> clientes = await _repository.GetAsync(Predicate(request), c => c.Carros);
            return clientes;
        }

        private Expression<Func<Cliente, bool>> Predicate(SelecionarClienteFiltersQuery request)
        {
            Expression<Func<Cliente, bool>> predicate = PredicateBuilder.True<Cliente>();
            
            if(!request.Nome.Equals(string.Empty))
                predicate = predicate.And(c => c.Nome.ToUpper().Contains(request.Nome.ToUpper()));
            if(!request.Email.Equals(string.Empty))
                predicate = predicate.And(c => c.Email.ToUpper().Contains(request.Email.ToUpper()));
            if(!request.Cpf.Equals(string.Empty))
                predicate = predicate.And(c => c.Cpf.ToUpper().Contains(request.Cpf.ToUpper()));
            if(!request.Telefone.Equals(string.Empty))
                predicate = predicate.And(c => c.Telefone.ToUpper().Contains(request.Cpf.ToUpper()));
            if (request.Ativo != null)
                predicate = predicate.And(c => c.Ativo.Equals(request.Ativo));

            return predicate;            
        }
    }

    public static class PredicateBuilder
    {
        public static Expression<Func<Cliente, bool>> True<Cliente>() { return c => true; }

        public static Expression<Func<Cliente, bool>> And<Cliente>(this Expression<Func<Cliente, bool>> expression1, Expression<Func<Cliente, bool>> expression2)
        {
            var invokedExpr = Expression.Invoke(expression2, expression1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<Cliente, bool>>
                            (Expression.And(expression1.Body, invokedExpr),expression1.Parameters);
        }
    }
}
