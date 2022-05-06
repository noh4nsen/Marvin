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
    public class CancelarAgendamentoCommand : IRequest<CancelarAgendamentoCommandResponse>
    {
        public Guid Id { get; set; }
    }

    public class CancelarAgendamentoCommandResponse
    {
        public Guid Id { get; set; }
    }

    public class CancelarAgendamentoCommandHandler : IRequestHandler<CancelarAgendamentoCommand, CancelarAgendamentoCommandResponse>
    {
        private readonly IRepository<Orcamento> _repository;
        private readonly CancelarAgendamentoCommandValidator _validator;

        public CancelarAgendamentoCommandHandler(IRepository<Orcamento> repository, CancelarAgendamentoCommandValidator validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<CancelarAgendamentoCommandResponse> Handle(CancelarAgendamentoCommand request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new ArgumentNullException("Erro ao processar requisição.");

            Orcamento entity = await GetOrcamentoAsync(request, cancellationToken);
            await CancelarAgendamentoAsync(entity);

            return entity.ToCancelarAgendamentoResponse();
        }

        private async Task CancelarAgendamentoAsync(Orcamento entity)
        {
            entity.ToCancelarAgendamento();
            await _repository.UpdateAsync(entity);
            await _repository.SaveChangesAsync();
        }

        private async Task<Orcamento> GetOrcamentoAsync(CancelarAgendamentoCommand request, CancellationToken cancellationToken)
        {
            Orcamento entity = await _repository.GetFirstAsync(o => o.Id.Equals(request.Id));
            await _validator.ValidateDataAsync(request, entity, cancellationToken);
            await _validator.ValidateDataAsync(request, entity.Status, cancellationToken);
            return entity;
        }
    }

    public class CancelarAgendamentoCommandValidator : AbstractValidator<CancelarAgendamentoCommand>
    {
        public CancelarAgendamentoCommandValidator()
        {
            RuleFor(r => r.Id)
                .NotEqual(Guid.Empty)
                .WithMessage("Id de orçamento é obrigatório");
        }

        public async Task ValidateDataAsync
            (
                CancelarAgendamentoCommand request,
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
                CancelarAgendamentoCommand request,
                StatusEnum status,
                CancellationToken cancellationToken
            )
        {
            RuleFor(r => r.Id)
                .Must(m => { return status == StatusEnum.Agendado; })
                .WithMessage($"Orçamento de id {request.Id} não está agendado ou já foi finalizado.");
            await this.ValidateAndThrowAsync(request, cancellationToken);
        }
    }
}
