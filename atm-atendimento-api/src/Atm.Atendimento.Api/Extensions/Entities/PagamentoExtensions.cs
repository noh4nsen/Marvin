using Atm.Atendimento.Api.Features.Orçamentos.Commands.InserirOrcamentoFeature;
using Atm.Atendimento.Api.Features.Orçamentos.Queries.SelecionarOrcamentoByIdFeature;
using Atm.Atendimento.Api.Helpers;
using Atm.Atendimento.Domain;
using System;

namespace Atm.Atendimento.Api.Extensions.Entities
{
    public static class PagamentoExtensions
    {
        public static Pagamento ToDomain(this InserirPagamentoCommand request)
        {
            return new Pagamento()
            {
                Id = Guid.NewGuid(),
                Ativo = true,
                Percentual = request.Percentual,
                Desconto = request.Desconto,
                ValorFinal = request.ValorFinal,
                PagamentoEfetuado = request.PagamentoEfetuado,
                ModoPagamento = request.ModoPagamento.ToDomain(),
                DataCadastro = DateHelper.GetLocalTime()
            };
        }

        public static ModoPagamento ToDomain(this InserirModoPagamentoCommand request)
        {
            return new ModoPagamento()
            {
                Id = Guid.NewGuid(),
                Ativo = true,
                CartaoCredito = request.CartaoCredito,
                CartaoDebito = request.CartaoDebito,
                Dinheiro = request.Dinheiro,
                Pix = request.Pix,
                DataCadastro = DateHelper.GetLocalTime()
            };
        }

        public static SelecionarPagamentoQueryResponse ToQueryResponse(this Pagamento entity)
        {
            return new SelecionarPagamentoQueryResponse()
            {
                Id = entity.Id,
                Percentual = entity.Percentual,
                Desconto = entity.Desconto,
                ValorFinal = entity.ValorFinal,
                PagamentoEfetuado = entity.PagamentoEfetuado,
                ModoPagamento = entity.ModoPagamento.ToQueryResponse()
            };
        }

        public static SelecionarModoPagamentoQueryResponse ToQueryResponse(this ModoPagamento entity)
        {
            return new SelecionarModoPagamentoQueryResponse()
            {
                Id = entity.Id,
                CartaoCredito = entity.CartaoCredito,
                CartaoDebito = entity.CartaoDebito,
                Dinheiro = entity.Dinheiro,
                Pix = entity.Pix
            };
        }
    }
}
