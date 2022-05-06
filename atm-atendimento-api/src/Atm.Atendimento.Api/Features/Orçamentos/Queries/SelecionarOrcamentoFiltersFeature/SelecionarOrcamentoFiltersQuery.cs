using Atm.Atendimento.Api.Features.Orçamentos.Queries.SelecionarOrcamentoByIdFeature;
using Atm.Atendimento.Domain;
using Atm.Atendimento.Domain.Enum;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Atm.Atendimento.Api.Features.Orçamentos.Queries.SelecionarOrcamentoFiltersFeature
{
    public class SelecionarOrcamentoFiltersQuery : IRequest<IEnumerable<SelecionarOrcamentoFiltersQueryResponse>>
    {
        public Guid ClienteId { get; set; }
        public Guid CarroId { get; set; }
        public StatusEnum Status { get; set; }
        public int? DiaCadastro { get; set; }
        public int? MesCadastro { get; set; }
        public int? AnoCadastro { get; set; }
        public int? DiaAgendamento { get; set; }
        public int? MesAgendamento { get; set; }
        public int? AnoAgendamento { get; set; }
    }

    public class SelecionarOrcamentoFiltersQueryValidator : AbstractValidator<SelecionarOrcamentoFiltersQuery>
    {
        public async Task ValidateDataAsync
            (
                SelecionarOrcamentoFiltersQuery request,
                CustoServico entity,
                CancellationToken cancellationToken
            )
        {
            RuleFor(r => r)
                .Must(m => { return entity is not null; })
                .WithMessage($"Custo Serviço não encontrado");
            await this.ValidateAndThrowAsync(request, cancellationToken);
        }

        public async Task ValidateDataAsync
            (
                SelecionarOrcamentoFiltersQuery request,
                Pagamento entity,
                CancellationToken cancellationToken
            )
        {
            RuleFor(r => r)
                .Must(m => { return entity is not null; })
                .WithMessage($"Pagamento não encontrado");
            await this.ValidateAndThrowAsync(request, cancellationToken);
        }
    }
}
