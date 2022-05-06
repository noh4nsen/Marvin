using Atm.Atendimento.Domain;
using System;

namespace Atm.Atendimento.Dto
{
    public class ProdutoOrcamento : Entity
    {
        public Guid IdExterno { get; set; }
        public int Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal Percentual { get; set; }
        public decimal ValorTotal { get; set; }
        public Orcamento Orcamento { get; set; }
    }
}
