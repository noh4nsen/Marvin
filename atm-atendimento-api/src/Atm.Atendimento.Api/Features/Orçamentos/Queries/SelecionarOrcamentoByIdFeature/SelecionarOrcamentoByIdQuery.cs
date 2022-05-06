using Atm.Atendimento.Domain;
using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Atm.Atendimento.Api.Features.Orçamentos.Queries.SelecionarOrcamentoByIdFeature
{
    public class SelecionarOrcamentoByIdQuery : IRequest<SelecionarOrcamentoByIdQueryResponse>
    {
        public Guid Id { get; set; }
    }

    public class SelecionarOrcamentoByIdQueryValidator : AbstractValidator<SelecionarOrcamentoByIdQuery>
    {
        public SelecionarOrcamentoByIdQueryValidator()
        {
            RuleFor(r => r.Id)
                .NotEqual(Guid.Empty)
                .WithMessage($"Id de orçamento é obrigatório.");
        }

        public async Task ValidateDataAsync
            (
                SelecionarOrcamentoByIdQuery request,
                Orcamento entity,
                CancellationToken cancellationToken
            )
        {
            RuleFor(r => r.Id)
                .Must(m => { return entity is not null; })
                .WithMessage($"Orçamento de id {request.Id} não encontrado");
            await this.ValidateAndThrowAsync(request, cancellationToken);
        }

        public async Task ValidateDataAsync
            (
                SelecionarOrcamentoByIdQuery request,
                CustoServico entity,
                CancellationToken cancellationToken
            )
        {
            RuleFor(r => r.Id)
                .Must(m => { return entity is not null; })
                .WithMessage($"Custo Serviço de id {request.Id} não encontrado");
            await this.ValidateAndThrowAsync(request, cancellationToken);
        }

        public async Task ValidateDataAsync
            (
                SelecionarOrcamentoByIdQuery request,
                Pagamento entity,
                CancellationToken cancellationToken
            )
        {
            RuleFor(r => r.Id)
                .Must(m => { return entity is not null; })
                .WithMessage($"Pagamento de id {request.Id} não encontrado");
            await this.ValidateAndThrowAsync(request, cancellationToken);
        }
    }
}
