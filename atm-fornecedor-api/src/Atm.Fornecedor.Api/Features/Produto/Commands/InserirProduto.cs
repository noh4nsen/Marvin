using Atm.Fornecedor.Api.Extensions.Entities;
using Atm.Fornecedor.Repositories;
using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Atm.Fornecedor.Api.Features.Produto.Commands
{
    public class InserirProdutoCommand : IRequest<InserirProdutoCommandResponse>
    {
        public string CodigoNCM { get; set; }
        public string Nome { get; set; }
        public string Tipo { get; set; }
        public string Descricao { get; set; }
        public int QuantidadeEstoque { get; set; }
        public decimal ValorUnitario { get; set; }
        public Domain.Fornecedor Fornecedor { get; set; }
    }

    public class InserirProdutoCommandResponse
    {
        public Guid Id { get; set; }
        public DateTime DataCadastro { get; set; }
    }

    public class InserirProdutoCommandHandler : IRequestHandler<InserirProdutoCommand, InserirProdutoCommandResponse>
    {
        private readonly IRepository<Domain.Produto> _repository;
        private readonly IRepository<Domain.Fornecedor> _repositoryFornecedor;
        private readonly InserirProdutoCommandValidator _validator;

        public InserirProdutoCommandHandler(IRepository<Domain.Produto> repository, IRepository<Domain.Fornecedor> repositoryFornecedor, InserirProdutoCommandValidator validator)
        {
            _repository = repository;
            _repositoryFornecedor = repositoryFornecedor;
            _validator = validator;
        }

        public async Task<InserirProdutoCommandResponse> Handle(InserirProdutoCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException("Erro ao processar requisição");

            Domain.Produto entity = request.ToDomain();

            await _validator.ValidateDataAsync(request, await _repositoryFornecedor.GetFirstAsync(f => f.Id.Equals(request.Fornecedor.Id)));


            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();

            return entity.ToInsertResponse();
        }
    }

    public class InserirProdutoCommandValidator : AbstractValidator<InserirProdutoCommand>
    {
        public InserirProdutoCommandValidator()
        {
            RuleFor(p => p.CodigoNCM).NotEmpty()
                                     .WithMessage("Código NCM de produto é obrigatório.");
            RuleFor(p => p.Nome).NotEmpty()
                                .WithMessage("Nome de produto é obrigatório.");
            RuleFor(p => p.Tipo).NotNull()
                                .WithMessage("Tipo do produto é obrigatório.");
            RuleFor(p => p.QuantidadeEstoque).GreaterThan(-1)
                                             .WithMessage("Quantidade em estoque não pode ser negativo.");
            RuleFor(p => p.ValorUnitario).NotNull()
                                         .WithMessage("Valor unitário é obrigatório.")
                                         .GreaterThan(0)
                                         .WithMessage("Valor unitário deve ser maior que zero.");
            RuleFor(p => p.Fornecedor.Id).NotEqual(Guid.Empty)
                                        .WithMessage("Id de Fornecedor é obrigatório.");
        }

        public async Task ValidateDataAsync(InserirProdutoCommand request, Domain.Fornecedor fornecedor)
        {
            RuleFor(r => r.Fornecedor.Id)
                .Must(f => { return fornecedor != null; })
                .WithMessage($"Fornecedor de id {request.Fornecedor.Id} não encontrado.");
            await this.ValidateAndThrowAsync(request);
        }
    }
}
