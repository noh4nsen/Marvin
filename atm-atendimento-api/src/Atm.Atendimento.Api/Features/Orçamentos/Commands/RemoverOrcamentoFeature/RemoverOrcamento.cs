using Atm.Atendimento.Api.Extensions.Entities;
using Atm.Atendimento.Domain;
using Atm.Atendimento.Repositories;
using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Atm.Atendimento.Api.Features.Orçamentos.Commands.RemoverOrcamentoFeature
{
    public class RemoverOrcamentoCommand : IRequest<RemoverOrcamentoCommandResponse>
    {
        public Guid Id { get; set; }
    }

    public class RemoverOrcamentoCommandResponse
    {
        public Guid Id { get; set; }
    }

    public class RemoverOrcamentoCommandHandler : IRequestHandler<RemoverOrcamentoCommand, RemoverOrcamentoCommandResponse>
    {
        private readonly IRepository<Orcamento> _repository;
        private readonly RemoverOrcamentoCommandValidator _validator;

        public RemoverOrcamentoCommandHandler
            (
                IRepository<Orcamento> repository,
                RemoverOrcamentoCommandValidator validator
            )
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<RemoverOrcamentoCommandResponse> Handle(RemoverOrcamentoCommand request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new ArgumentNullException("Erro ao processar requisição.");

            Orcamento entity = await GetOrcamentoAsync(request, cancellationToken);
            await RemoveOrcamentoAsync(entity);

            return entity.ToRemoveResponse();
        }

        private async Task RemoveOrcamentoAsync(Orcamento entity)
        {
            await _repository.RemoveAsync(entity);
            await _repository.SaveChangesAsync();
        }

        private async Task<Orcamento> GetOrcamentoAsync(RemoverOrcamentoCommand request, CancellationToken cancellationToken)
        {
            Orcamento entity = await _repository.GetFirstAsync(o => o.Id.Equals(request.Id));
            await _validator.ValidateDataAsync(request, entity, cancellationToken);
            return entity;
        }
    }

    public class RemoverOrcamentoCommandValidator : AbstractValidator<RemoverOrcamentoCommand>
    {
        public RemoverOrcamentoCommandValidator()
        {
            RuleFor(r => r.Id)
                .NotEqual(Guid.Empty)
                .WithMessage("Id de Orçamento é obrigatório.");
        }

        public async Task ValidateDataAsync
            (
                RemoverOrcamentoCommand request,
                Orcamento entity,
                CancellationToken cancellationToken
            )
        {
            RuleFor(r => r.Id)
                .Must(m => { return entity is not null; })
                .WithMessage($"Orçamento de id {request.Id} não encontrado.");
            await this.ValidateAndThrowAsync(request, cancellationToken);
        }
    }
}
