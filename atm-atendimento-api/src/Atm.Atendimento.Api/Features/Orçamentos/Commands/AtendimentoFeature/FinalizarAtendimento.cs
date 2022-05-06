using Atm.Atendimento.Api.Extensions.Entities;
using Atm.Atendimento.Api.Helpers;
using Atm.Atendimento.Domain;
using Atm.Atendimento.Repositories;
using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Atm.Atendimento.Api.Features.Orçamentos.Commands.AtendimentoFeature
{
    public class FinalizarAtendimentoCommand : IRequest<FinalizarAtendimentoCommandResponse>
    {
        public Guid Id { get; set; }
        public DateTime? DataHoraFim { get; set; }
    }

    public class FinalizarAtendimentoCommandResponse
    {
        public Guid Id { get; set; }
        public DateTime? DataHoraFim { get; set; }
        public double? Duracao { get; set; }
    }

    public class FinalizarAtendimentoCommandHandler : IRequestHandler<FinalizarAtendimentoCommand, FinalizarAtendimentoCommandResponse>
    {
        private readonly IRepository<Orcamento> _repository;
        private readonly FinalizarAtendimentoCommandValidator _validator;

        public FinalizarAtendimentoCommandHandler
            (
                IRepository<Orcamento> repository,
                FinalizarAtendimentoCommandValidator validator
            )
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<FinalizarAtendimentoCommandResponse> Handle(FinalizarAtendimentoCommand request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new ArgumentNullException("Erro ao processar requisição.");

            Orcamento entity = await GetOrcamentoAsync(request, cancellationToken);
            await ValidateDataFinalizacao(request, entity, cancellationToken);
            await FinalizarAtendimentoAsync(request, entity);

            return entity.ToFinalizarAtendimentoResponse();
        }

        private async Task FinalizarAtendimentoAsync(FinalizarAtendimentoCommand request, Orcamento entity)
        {
            entity.ToFinalizarAtendimento(request);
            await _repository.UpdateAsync(entity);
            await _repository.SaveChangesAsync();
        }

        private async Task ValidateDataFinalizacao(FinalizarAtendimentoCommand request, Orcamento entity, CancellationToken cancellationToken)
        {
            await _validator.ValidateDataAsync(request, DateHelper.GetLocalTime() > entity.DataAgendamento, cancellationToken);
        }

        private async Task<Orcamento> GetOrcamentoAsync(FinalizarAtendimentoCommand request, CancellationToken cancellationToken)
        {
            Orcamento entity = await _repository.GetFirstAsync(o => o.Id.Equals(request.Id));
            await _validator.ValidateDataAsync(request, entity, cancellationToken);
            return entity;
        }
    }

    public class FinalizarAtendimentoCommandValidator : AbstractValidator<FinalizarAtendimentoCommand>
    {
        public FinalizarAtendimentoCommandValidator()
        {
            RuleFor(r => r.Id)
                .NotEqual(Guid.Empty)
                .WithMessage("Id de orçamento é obrigatório.");
        }

        public async Task ValidateDataAsync
            (
                FinalizarAtendimentoCommand request,
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
                FinalizarAtendimentoCommand request,
                bool valido,
                CancellationToken cancellationToken
            )
        {
            RuleFor(r => r.Id)
                .Must(m => valido)
                .WithMessage("Data de finalização precede data de agendamento.");
            await this.ValidateAndThrowAsync(request, cancellationToken);
        }
    }
}
