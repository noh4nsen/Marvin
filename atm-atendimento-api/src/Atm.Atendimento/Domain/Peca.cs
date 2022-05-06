namespace Atm.Atendimento.Domain
{
    public class Peca : Entity
    {
        public string CodigoNCM { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal ValorUnitarioCompra{ get; set; }
        public decimal ValorUnitarioVenda { get; set; }
        public int Quantidade { get; set; }
        public decimal Percentual { get; set; }
        public decimal ValorCobrado { get; set; }
        public Orcamento Orcamento { get; set; }
    }
}
