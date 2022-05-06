using Atm.Atendimento.Domain.Enum;
using Atm.Atendimento.Dto;
using System;
using System.Collections.Generic;

namespace Atm.Atendimento.Domain
{
    public class Orcamento : Entity
    {
        public ClienteOrcamento Cliente { get; set; }
        public CarroOrcamento Carro { get; set; }
        public ICollection<ProdutoOrcamento> Produtos { get; set; }
        public ICollection<Peca> Pecas { get; set; }
        public ICollection<CustoServico> CustoServicos { get; set; }
        public string Descricao { get; set; }
        public Pagamento Pagamento { get; set; }
        public StatusEnum Status { get; set; }
        public DateTime? DataAgendamento { get; set; }
        public DateTime? DataHoraInicio { get; set; }
        public DateTime? DataHoraFim { get; set; }
        public double? Duracao { get; set; }

        // Pode ser usado para calcular duracao
        //public string tteste()
        //{
        //    var a = ((DateTime)DataHoraInicio - (DateTime)DataHoraFim).TotalHours;
        //}
    }
}
