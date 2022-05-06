using Atm.Fornecedor.Api.Extensions.Entities;
using Atm.Fornecedor.Repositories;
using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Atm.Fornecedor.Api.Features.Fornecedor.Commands
{
    public class RemoverFornecedorCommand : IRequest<RemoverFornecedorCommandResponse>
    {
        public Guid Id { get; set; }
    }

    public class RemoverFornecedorCommandResponse
    {
        public Guid Id { get; set; }
    }

    public class RemoverFornecedorCommandHandler : IRequestHandler<RemoverFornecedorCommand, RemoverFornecedorCommandResponse>
    {
        private readonly IRepository<Domain.Fornecedor> _repositoryFornecedor;
        private readonly IRepository<Domain.Produto> _repositoryProduto;
        private readonly RemoverFornecedorCommandValidator _validator;

        public RemoverFornecedorCommandHandler(IRepository<Domain.Fornecedor> repositoryFornecedor, IRepository<Domain.Produto> repositoryProduto, RemoverFornecedorCommandValidator validator)
        {
            _repositoryFornecedor = repositoryFornecedor;
            _repositoryProduto = repositoryProduto;
            _validator = validator;
        }

        public async Task<RemoverFornecedorCommandResponse> Handle(RemoverFornecedorCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException("Erro ao processar requisição");

            Domain.Fornecedor entity = await _repositoryFornecedor.GetFirstAsync(f => f.Id.Equals(request.Id));

            await _validator.ValidateDataAsync(request, entity);
            await _validator.ValidateDataAsync(request, await _repositoryProduto.GetFirstAsync(p => p.FornecedorId.Equals(entity.Id)));

            await _repositoryFornecedor.RemoveAsync(entity);


            await _repositoryFornecedor.SaveChangesAsync();

            return entity.ToRemoveResponse();
        }
    }

    public class RemoverFornecedorCommandValidator : AbstractValidator<RemoverFornecedorCommand>
    {
        public RemoverFornecedorCommandValidator()
        {
            RuleFor(f => f.Id)
                .NotEqual(Guid.Empty)
                .WithMessage("Id de fornecedor é obrigatório");
        }

        public async Task ValidateDataAsync(RemoverFornecedorCommand request, Domain.Fornecedor entity)
        {
            RuleFor(r => r.Id)
               .Must(f => { return entity != null; })
               .WithMessage($"Fornecedor de id {request.Id} não encontrado.");
            await this.ValidateAndThrowAsync(request);
        }

        public async Task ValidateDataAsync(RemoverFornecedorCommand request, Domain.Produto entity)
        {
            RuleFor(r => r.Id)
                .Must(p => { return entity == null; })
                .WithMessage($"Fornecedor de id {request.Id} possui produtos vinculados.");
            await this.ValidateAndThrowAsync(request);
        }
    }
}
