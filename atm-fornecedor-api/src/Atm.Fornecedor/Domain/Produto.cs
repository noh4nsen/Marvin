using System;
using System.Collections.Generic;

namespace Atm.Fornecedor.Domain
{
    public class Produto : Entity
    {
        public string CodigoNCM { get; set; }
        public string Nome { get; set; }
        public string Tipo { get; set; }
        public string Descricao { get; set; }
        public int QuantidadeEstoque { get; set; }
        public decimal ValorAtual { get; set; }
        public Guid FornecedorId { get; set; }
        public Fornecedor Fornecedor { get; set; }
        public Guid HistoricoProdutoAtual { get; set; }
        public ICollection<HistoricoProduto> HistoricoProduto { get; set; }
    }
}