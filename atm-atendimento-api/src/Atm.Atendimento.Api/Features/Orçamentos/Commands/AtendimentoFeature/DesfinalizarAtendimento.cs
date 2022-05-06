using Atm.Atendimento.Api.Extensions.Entities;
using Atm.Atendimento.Domain;
using Atm.Atendimento.Domain.Enum;
using Atm.Atendimento.Repositories;
using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Atm.Atendimento.Api.Features.Orçamentos.Commands.AtendimentoFeature
{
    public class DesfinalizarAtendimentoCommand : IRequest<DesfinalizarAtendimentoCommandResponse>
    {
        public Guid Id { get; set; }
    }

    public class DesfinalizarAtendimentoCommandResponse
    {
        public Guid Id { get; set; }
    }

    public class DesfinalizarAtendimentoCommandHandler : IRequestHandler<DesfinalizarAtendimentoCommand, DesfinalizarAtendimentoCommandResponse>
    {
        private readonly IRepository<Orcamento> _repository;
        private readonly DesfinalizarAtendimentoCommandValidator _validator;

        public DesfinalizarAtendimentoCommandHandler(IRepository<Orcamento> repository, DesfinalizarAtendimentoCommandValidator validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<DesfinalizarAtendimentoCommandResponse> Handle(DesfinalizarAtendimentoCommand request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new ArgumentNullException("Erro ao processar requisição.");

            Orcamento entity = await GetOrcamentoAsync(request, cancellationToken);
            await DesfinalizarAtendimento(entity);

            return entity.ToDesfinalizarAtendimentoResponse();
        }

        private async Task DesfinalizarAtendimento(Orcamento entity)
        {
            entity.ToDesfinalizarAtendimento();
            await _repository.UpdateAsync(entity);
            await _repository.SaveChangesAsync();
        }

        private async Task<Orcamento> GetOrcamentoAsync(DesfinalizarAtendimentoCommand request, CancellationToken cancellationToken)
        {
            Orcamento entity = await _repository.GetFirstAsync(o => o.Id.Equals(request.Id));
            await _validator.ValidateDataAsync(request, entity, cancellationToken);
            await _validator.ValidateDataAsync(request, entity.Status, cancellationToken);
            return entity;
        }
    }

    public class DesfinalizarAtendimentoCommandValidator : AbstractValidator<DesfinalizarAtendimentoCommand>
    {
        public DesfinalizarAtendimentoCommandValidator()
        {
            RuleFor(r => r.Id)
                .NotEqual(Guid.Empty)
                .WithMessage("Id de orçamento é obrigatório");
        }

        public async Task ValidateDataAsync
            (
                DesfinalizarAtendimentoCommand request,
                Orcamento entity,
                CancellationToken cancellationToken
            )
        {
            RuleFor(r => r.Id)
                .Must(m => { return entity is not null; })
                .WithMessage($"Orçamento de id {request.Id} não encontrado.");
            await this.ValidateAndThrowAsync(request, cancellationToken);
        }

        public async Task ValidateDataAsync
            (
                DesfinalizarAtendimentoCommand request,
                StatusEnum status,
                CancellationToken cancellationToken
            )
        {
            RuleFor(r => r.Id)
                .Must(m => { return status == StatusEnum.Finalizado; })
                .WithMessage($"Agendamento de id {request.Id} ainda não foi finalizado.");
            await this.ValidateAndThrowAsync(request, cancellationToken);
        }
    }
}
