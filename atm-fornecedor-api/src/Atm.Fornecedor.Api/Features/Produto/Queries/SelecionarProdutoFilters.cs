using Atm.Fornecedor.Api.Extensions.Entities;
using Atm.Fornecedor.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Atm.Fornecedor.Api.Features.Produto.Queries
{
    public class SelecionarProdutoFiltersQuery : IRequest<IEnumerable<SelecionarProdutoQueryResponse>>
    {
        public string Nome { get; set; }
        public bool? Ativo { get; set; }
    }

    public class SelecionarProdutoFiltersQueryHandler : IRequestHandler<SelecionarProdutoFiltersQuery, IEnumerable<SelecionarProdutoQueryResponse>>
    {
        private readonly IRepository<Domain.Produto> _repositoryProduto;
        private readonly IRepository<Domain.Fornecedor> _repositoryFornecedor;

        public SelecionarProdutoFiltersQueryHandler(IRepository<Domain.Produto> repositoryProduto, IRepository<Domain.Fornecedor> repositoryFornecedor)
        {
            _repositoryProduto = repositoryProduto;
            _repositoryFornecedor = repositoryFornecedor;
        }

        public async Task<IEnumerable<SelecionarProdutoQueryResponse>> Handle(SelecionarProdutoFiltersQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException("Erro ao processar requisição");

            IEnumerable<Domain.Produto> result = await _repositoryProduto.GetAsync(Predicate(request));

            IEnumerable<Domain.Fornecedor> listaFornecedores = await PreencheFornecedores(result);

            return result.ToQueryResponse(listaFornecedores);
        }

        private Expression<Func<Domain.Produto, bool>> Predicate(SelecionarProdutoFiltersQuery request)
        {
            Expression<Func<Domain.Produto, bool>> predicate = PredicateBuilder.True<Domain.Produto>();

            if (!request.Nome.Equals(string.Empty))
                predicate = predicate.And(p => p.Nome.ToUpper().Contains(request.Nome.ToUpper()));
            if (request.Ativo is not null)
                predicate = predicate.And(p => p.Ativo.Equals(request.Ativo));
            return predicate;
        }

        private async Task<IEnumerable<Domain.Fornecedor>> PreencheFornecedores(IEnumerable<Domain.Produto> produtosList)
        {
            IList<Domain.Fornecedor> listaFornecedores = new List<Domain.Fornecedor>();
            foreach (var produto in produtosList)
            {
                listaFornecedores.Add(await _repositoryFornecedor.GetFirstAsync(f => f.Id.Equals(produto.FornecedorId)));
            }

            return listaFornecedores;
        }
    }

    public static class PredicateBuilder
    {
        public static Expression<Func<Domain.Produto, bool>> True<Produto>() { return c => true; }

        public static Expression<Func<Domain.Produto, bool>> And<Produto>(this Expression<Func<Produto, bool>> expression1, Expression<Func<Produto, bool>> expression2)
        {
            var invokedExpr = Expression.Invoke(expression2, expression1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<Domain.Produto, bool>>
                            (Expression.And(expression1.Body, invokedExpr), expression1.Parameters);
        }
    }
}
