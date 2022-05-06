using Atm.Fornecedor.Api.Extensions.Entities;
using Atm.Fornecedor.Repositories;
using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Atm.Fornecedor.Api.Features.Produto.Commands
{
    public class RemoverProdutoCommand : IRequest<RemoverProdutoCommandResponse>
    {
        public Guid Id { get; set; }
    }

    public class RemoverProdutoCommandResponse
    {
        public Guid Id { get; set; }
    }

    public class RemoverProdutoCommandHandler : IRequestHandler<RemoverProdutoCommand, RemoverProdutoCommandResponse>
    {
        private readonly IRepository<Domain.Produto> _repository;
        private readonly RemoverProdutoCommandValidator _validator;

        public RemoverProdutoCommandHandler(IRepository<Domain.Produto> repository, RemoverProdutoCommandValidator validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<RemoverProdutoCommandResponse> Handle(RemoverProdutoCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException("Erro ao processar requisição");

            Domain.Produto entity = await _repository.GetFirstAsync(p => p.Id.Equals(request.Id), p => p.HistoricoProduto);

            await _validator.ValidateDataAsync(request, entity);

            entity.Ativo = false;
            await _repository.UpdateAsync(entity);
            await _repository.SaveChangesAsync();

            return entity.ToRemoveResponse();
        }
    }

    public class RemoverProdutoCommandValidator : AbstractValidator<RemoverProdutoCommand>
    {
        public RemoverProdutoCommandValidator()
        {
            RuleFor(p => p.Id)
                .NotEqual(Guid.Empty)
                .WithMessage("Id de produto é obrigatório");
        }

        public async Task ValidateDataAsync(RemoverProdutoCommand request, Domain.Produto entity)
        {
            RuleFor(r => r.Id)
                .Must(p => { return entity != null; })
                .WithMessage($"Produto de id {request.Id} não encontrado.");
            await this.ValidateAndThrowAsync(request);
        }
    }
}
