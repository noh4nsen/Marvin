using System;
using System.Collections.Generic;

namespace Atm.Atendimento.Domain
{
    public class Servico : Entity
    {
        public string Nome { get; set; }
        public decimal? ValorAtual { get; set; }
        public Guid? CustoServicoAtual { get; set; }
        public ICollection<CustoServico> CustoServico { get; set; }
    }
}
