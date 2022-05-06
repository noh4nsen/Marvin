namespace Atm.Fornecedor.Domain
{
    public class HistoricoProduto : Entity
    {
        public decimal ValorUnitario { get; set; }
        public Produto Produto { get; set; }
    }
}
