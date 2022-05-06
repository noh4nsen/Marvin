using Atm.Fornecedor.Api.Extensions.Entities;
using Atm.Fornecedor.Repositories;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Atm.Fornecedor.Api.Features.Produto.Queries
{
    public class SelecionarProdutoNamesQuery : IRequest<IEnumerable<SelecionarProdutoNamesQueryResponse>>
    {
        public string Nome { get; set; }
        public bool? Ativo { get; set; }
    }

    public class SelecionarProdutoNamesQueryResponse
    {
        public string Nome { get; set; }
    }

    public class SelecionarProdutoNamesQueryHandler : IRequestHandler<SelecionarProdutoNamesQuery, IEnumerable<SelecionarProdutoNamesQueryResponse>>
    {
        private readonly IRepository<Domain.Produto> _repository;

        public SelecionarProdutoNamesQueryHandler
            (
                IRepository<Domain.Produto> repository
            )
        {
            _repository = repository;
        }

        public async Task<IEnumerable<SelecionarProdutoNamesQueryResponse>> Handle(SelecionarProdutoNamesQuery request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new ArgumentNullException("Erro ao processar requisição");

            IEnumerable<Domain.Produto> result = await _repository.GetAsync(Predicate(request));

            return result.ToNameResponse();
        }

        private Expression<Func<Domain.Produto, bool>> Predicate(SelecionarProdutoNamesQuery request)
        {
            Expression<Func<Domain.Produto, bool>> predicate = PredicateBuilder.True<Domain.Produto>();

            if (!request.Nome.Equals(string.Empty))
                predicate = predicate.And(p => p.Nome.ToUpper().Contains(request.Nome.ToUpper()));
            if (request.Ativo is not null)
                predicate = predicate.And(p => p.Ativo.Equals(request.Ativo));
            return predicate;
        }
    }

    public class SelecionarProdutoNamesQueryValidator : AbstractValidator<SelecionarProdutoNamesQuery>
    {
        public SelecionarProdutoNamesQueryValidator()
        {
            RuleFor(p => p.Nome).NotEmpty()
                    .WithMessage("Nome de produto é obrigatório");
        }
    }
}
