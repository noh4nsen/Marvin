namespace Atm.Atendimento.Domain
{
    public class CustoServico : Entity
    {
        public decimal Valor { get; set; }
        public string Descricao { get; set; }
        public Servico Servico { get; set; }
        public Orcamento Orcamento { get; set; }
    }
}
