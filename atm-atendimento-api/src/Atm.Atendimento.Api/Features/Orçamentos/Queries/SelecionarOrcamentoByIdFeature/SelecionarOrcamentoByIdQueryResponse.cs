using Atm.Atendimento.Domain.Enum;
using System;
using System.Collections.Generic;

namespace Atm.Atendimento.Api.Features.Orçamentos.Queries.SelecionarOrcamentoByIdFeature
{
    public class SelecionarOrcamentoByIdQueryResponse
    {
        public Guid Id { get; set; }
        public Guid ClienteId { get; set; }
        public Guid CarroId { get; set; }
        public string Descricao { get; set; }
        public decimal ValorFinal { get; set; }
        public StatusEnum Status { get; set; }
        public ICollection<SelecionarProdutoQueryResponse> Produtos { get; set; }
        public ICollection<SelecionarPecaQueryResponse> Pecas { get; set; }
        public ICollection<SelecionarCustoServicoQueryResponse> Servicos { get; set; }
        public SelecionarPagamentoQueryResponse Pagamento { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAgendamento { get; set; }
        public DateTime? DataHoraInicio { get; set; }
        public DateTime? DataHoraFim { get; set; }
        public double? Duracao { get; set; }
    }

    public class SelecionarProdutoQueryResponse
    {
        public Guid Id { get; set; }
        public Guid ProdutoId { get; set; }
        public decimal ValorUnitario { get; set; }  
        public int Quantidade { get; set; }
        public decimal Percentual { get; set; }
        public decimal ValorTotal { get; set; }
    }

    public class SelecionarPecaQueryResponse
    {
        public Guid Id { get; set; }
        public string CodigoNCM { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal ValorUnitarioCompra { get; set; }
        public decimal ValorUnitarioVenda { get; set; }
        public int Quantidade { get; set; }
        public decimal Percentual { get; set; }
        public decimal ValorCobrado { get; set; }
    }

    public class SelecionarCustoServicoQueryResponse
    {
        public Guid Id { get; set; }
        public Guid ServicoId { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
    }

    public class SelecionarPagamentoQueryResponse
    {
        public Guid Id { get; set; }
        public decimal? Percentual { get; set; }
        public decimal? Desconto { get; set; }
        public decimal ValorFinal { get; set; }
        public bool PagamentoEfetuado { get; set; }
        public SelecionarModoPagamentoQueryResponse ModoPagamento { get; set; }
    }

    public class SelecionarModoPagamentoQueryResponse
    {
        public Guid Id { get; set; }
        public bool CartaoCredito { get; set; }
        public bool CartaoDebito { get; set; }
        public bool Dinheiro { get; set; }
        public bool Pix { get; set; }
    }
}
