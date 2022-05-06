using Atm.Fornecedor.Api.Extensions.Entities;
using Atm.Fornecedor.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Atm.Fornecedor.Api.Features.Produto.Queries
{
    public class SelecionarProdutoByIdQuery : IRequest<SelecionarProdutoQueryResponse>
    {
        public Guid Id { get; set; }
    }

    public class SelecionarProdutoQueryResponse
    {
        public Guid Id { get; set; }
        public bool Ativo { get; set; }
        public string CodigoNCM { get; set; }
        public string Nome { get; set; }
        public string Tipo { get; set; }
        public string Descricao { get; set; }
        public int QuantidadeEstoque { get; set; }
        public decimal ValorUnitario { get; set; }
        public SelecionarProdutoFornecedorQueryResponse Fornecedor { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAtualizacao { get; set; }
    }

    public class SelecionarProdutoFornecedorQueryResponse
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Cnpj { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string Tipo { get; set; }
        public string Endereco { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAtualizacao { get; set; }
    }

    public class SelecionarProdutoByIdQueryHandler : IRequestHandler<SelecionarProdutoByIdQuery, SelecionarProdutoQueryResponse>
    {
        private readonly IRepository<Domain.Produto> _repositoryProduto;
        private readonly IRepository<Domain.Fornecedor> _repositoryFornecedor;
        private readonly SelecionarProdutoByIdQueryValidator _validator;

        public SelecionarProdutoByIdQueryHandler(IRepository<Domain.Produto> repositoryProduto, IRepository<Domain.Fornecedor> repositoryFornecedor, SelecionarProdutoByIdQueryValidator validator)
        {
            _repositoryProduto = repositoryProduto;
            _repositoryFornecedor = repositoryFornecedor;
            _validator = validator;
        }

        public async Task<SelecionarProdutoQueryResponse> Handle(SelecionarProdutoByIdQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException("Erro ao processar requisição");

            Domain.Produto entity = await _repositoryProduto.GetFirstAsync(p => p.Id.Equals(request.Id));
            await _validator.ValidateDataAsync(request, entity);

            Domain.Fornecedor fornecedor = await _repositoryFornecedor.GetFirstAsync(f => f.Id.Equals(entity.FornecedorId));

            return entity.ToQueryResponse(fornecedor);
        }
    }

    public class SelecionarProdutoByIdQueryValidator : AbstractValidator<SelecionarProdutoByIdQuery>
    {
        public SelecionarProdutoByIdQueryValidator()
        {
            RuleFor(p => p.Id)
                .NotEqual(Guid.Empty)
                .WithMessage("Id inválido.");
        }

        public async Task ValidateDataAsync(SelecionarProdutoByIdQuery request, Domain.Produto entity)
        {
            RuleFor(r => r.Id)
                .Must(p => { return entity != null; })
                .WithMessage($"Produto de id {request.Id} não encontrado.");
            await this.ValidateAndThrowAsync(request);
        }
    }
}
