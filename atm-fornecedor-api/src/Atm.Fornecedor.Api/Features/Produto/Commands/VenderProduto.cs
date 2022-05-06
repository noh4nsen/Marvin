using Atm.Fornecedor.Api.Extensions.Entities;
using Atm.Fornecedor.Repositories;
using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Atm.Fornecedor.Api.Features.Produto.Commands
{
    public class VenderProdutoCommand : IRequest<VenderProdutoCommandResponse>
    {
        public Guid Id { get; set; }
        public int Quantidade { get; set; }
    }

    public class VenderProdutoCommandResponse
    {
        public Guid Id { get; set; }
    }

    public class VenderProdutoCommandHandler : IRequestHandler<VenderProdutoCommand, VenderProdutoCommandResponse>
    {
        private readonly IRepository<Domain.Produto> _repository;
        private readonly VenderProdutoCommandValidator _validator;

        public VenderProdutoCommandHandler(IRepository<Domain.Produto> repository, VenderProdutoCommandValidator validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<VenderProdutoCommandResponse> Handle(VenderProdutoCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException("Erro ao processar requisição");

            Domain.Produto entity = await _repository.GetFirstAsync(p => p.Id.Equals(request.Id));
            await _validator.ValidateDataAsync(request, entity);

            request.VenderProduto(entity);

            await _repository.UpdateAsync(entity);
            await _repository.SaveChangesAsync();

            return new VenderProdutoCommandResponse() { Id = entity.Id };
        }
    }

    public class VenderProdutoCommandValidator : AbstractValidator<VenderProdutoCommand>
    {
        public VenderProdutoCommandValidator()
        {
            RuleFor(p => p.Id).NotEqual(Guid.Empty)
                              .WithMessage("Id de Produto é obrigatório.");
            RuleFor(p => p.Quantidade).GreaterThan(0)
                                      .WithMessage("Quantidade de venda de produto deve ser maior que zero.");
        }

        public async Task ValidateDataAsync(VenderProdutoCommand request, Domain.Produto entity)
        {
            RuleFor(r => r.Id)
                .Must(p => { return entity != null; })
                .WithMessage($"Produto de id {request.Id} não encontrado.");
            await this.ValidateAndThrowAsync(request);
        }
    }
}
