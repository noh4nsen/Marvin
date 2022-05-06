namespace Atm.Atendimento.Domain
{
    public class Pagamento : Entity
    {
        public decimal? Percentual { get; set; }
        public decimal? Desconto { get; set; }
        public decimal ValorFinal { get; set; }
        public bool PagamentoEfetuado { get; set; }
        public ModoPagamento ModoPagamento { get; set; }
    }
}
