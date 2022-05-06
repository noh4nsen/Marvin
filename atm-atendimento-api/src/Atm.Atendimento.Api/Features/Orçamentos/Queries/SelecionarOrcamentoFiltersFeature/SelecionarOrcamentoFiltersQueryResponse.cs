using Atm.Atendimento.Domain.Enum;
using System;

namespace Atm.Atendimento.Api.Features.Orçamentos.Queries.SelecionarOrcamentoFiltersFeature
{
    public class SelecionarOrcamentoFiltersQueryResponse
    {
        public Guid Id { get; set; }
        public Guid ClienteId { get; set; }
        public Guid CarroId { get; set; }
        public decimal ValorFinal { get; set; }
        public StatusEnum Status { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAgendamento { get; set; }
    }
}
