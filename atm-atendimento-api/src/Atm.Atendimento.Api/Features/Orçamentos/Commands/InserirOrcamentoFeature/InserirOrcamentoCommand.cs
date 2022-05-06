using Atm.Atendimento.Domain;
using Atm.Atendimento.Dto;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Atm.Atendimento.Api.Features.Orçamentos.Commands.InserirOrcamentoFeature
{
    public class InserirOrcamentoCommand : IRequest<InserirOrcamentoCommandResponse>
    {
        public Guid ClienteId { get; set; }
        public Guid CarroId { get; set; }
        public string Descricao { get; set; }
        public ICollection<InserirProdutoCommand> Produtos { get; set; }
        public ICollection<InserirPecaCommand> Pecas { get; set; }
        public ICollection<InserirCustoServicoCommand> Servicos { get; set; }
        public InserirPagamentoCommand Pagamento { get; set; }
    }

    public class InserirProdutoCommand
    {
        public Guid ProdutoId { get; set; }
        public int Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal Percentual { get; set; }
        public decimal ValorTotal { get; set; }
    }

    public class InserirPecaCommand
    {
        public string CodigoNCM { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal ValorUnitarioCompra { get; set; }
        public decimal ValorUnitarioVenda { get; set; }
        public int Quantidade { get; set; }
        public decimal Percentual { get; set; }
        public decimal ValorCobrado { get; set; }
    }

    public class InserirCustoServicoCommand
    {
        public Guid ServicoId { get; set; }
        public decimal Valor { get; set; }
        public string Descricao { get; set; }
    }

    public class InserirPagamentoCommand
    {
        public decimal? Percentual { get; set; }
        public decimal? Desconto { get; set; }
        public decimal ValorFinal { get; set; }
        public bool PagamentoEfetuado { get; set; }
        public InserirModoPagamentoCommand ModoPagamento { get; set; }
    }

    public class InserirModoPagamentoCommand
    {
        public bool CartaoCredito { get; set; }
        public bool CartaoDebito { get; set; }
        public bool Dinheiro { get; set; }
        public bool Pix { get; set; }
    }

    public class InserirOrcamentoCommandValidator : AbstractValidator<InserirOrcamentoCommand>
    {
        public InserirOrcamentoCommandValidator()
        {
            RuleFor(r => r.ClienteId)
                .NotEqual(Guid.Empty)
                .WithMessage("Id de cliente é obrigatório.");

            RuleFor(r => r.CarroId)
                .NotEqual(Guid.Empty)
                .WithMessage("Id de carro é obrigatório.");

            RuleForEach(r => r.Produtos)
                .ChildRules(produto =>
                {
                    produto.RuleFor(p => p.ProdutoId)
                           .NotEqual(Guid.Empty)
                           .WithMessage("Id de produto é obrigatório");
                    produto.RuleFor(p => p.Quantidade)
                           .NotNull()
                           .WithMessage("Quantidade de produto é obrigatório.")
                           .GreaterThan(0)
                           .WithMessage("Quantidade de produto deve ser maior que zero.");
                    produto.RuleFor(p => p.ValorUnitario)
                           .NotNull()
                           .WithMessage("Valor unitário de produto é obrigatório.")
                           .GreaterThan(0)
                           .WithMessage("Valor unitário de produto deve ser maior que zero.");
                    produto.RuleFor(p => p.Percentual)
                           .NotNull()
                           .WithMessage("Percentual de valor produto é obrigatório.");
                    produto.RuleFor(p => p.ValorTotal)
                           .NotNull()
                           .WithMessage("Valor total de produto é obrigatório.")
                           .GreaterThan(0)
                           .WithMessage("Valor total de produto deve zer maior que zero.");
                });

            RuleForEach(r => r.Pecas)
                .ChildRules(peca =>
                {
                    peca.RuleFor(p => p.CodigoNCM)
                        .NotEmpty()
                        .WithMessage("Código NCM de peça é obrigatório.");
                    peca.RuleFor(p => p.Nome)
                        .NotEmpty()
                        .WithMessage("Nome de peça é obrigatório.");
                    peca.RuleFor(p => p.ValorUnitarioCompra)
                        .NotNull()
                        .WithMessage("Valor unitário de compra de peça é obrigatório.")
                        .GreaterThan(0)
                        .WithMessage("Valor unitário de compra de peça deve ser maior que zero.");
                    peca.RuleFor(p => p.ValorUnitarioVenda)
                        .NotNull()
                        .WithMessage("Valor unitário de venda de peça é obrigatório.")
                        .GreaterThan(0)
                        .WithMessage("Valor unitário de venda de peça deve ser maior que zero.");
                    peca.RuleFor(p => p.Quantidade)
                        .NotNull()
                        .WithMessage("Quantidade de peças é obrigatório.")
                        .GreaterThan(0)
                        .WithMessage("Quantidade de peças deve ser maior que zero.");
                    peca.RuleFor(p => p.Percentual)
                        .NotNull()
                        .WithMessage("Percentual de valor de peça é obrigatório.");
                    peca.RuleFor(p => p.ValorCobrado)
                        .NotNull()
                        .WithMessage("Valor cobrado de peça é obrigatório.")
                        .GreaterThan(0)
                        .WithMessage("Valor total de peça deve zer maior que zero.");
                });

            RuleForEach(r => r.Servicos)
                .ChildRules(servico =>
                {
                    servico.RuleFor(r => r.ServicoId)
                           .NotEqual(Guid.Empty)
                           .WithMessage("Id de serviço é obrigatório.");
                    servico.RuleFor(r => r.Valor)
                           .NotNull()
                           .WithMessage("Valor cobrado de serviço é obrigatório.");
                });

            RuleFor(r => r.Pagamento)
                .NotNull()
                .WithMessage("Pagamento é obrigatório.")
                .ChildRules(pagamento =>
                {
                    pagamento.RuleFor(p => p.ValorFinal)
                             .NotNull()
                             .WithMessage("Valor final de Pagamento é obrigatório.");
                    pagamento.RuleFor(p => p.PagamentoEfetuado)
                             .NotNull()
                             .WithMessage("Estado de pagamento efetuado é obrigatório.");
                    pagamento.RuleFor(p => p.ModoPagamento)
                             .ChildRules(modoPagamento =>
                             {
                                 modoPagamento.RuleFor(mp => mp.CartaoCredito)
                                              .NotNull()
                                              .WithMessage("Modo de pagamento é obrigatório.");
                                 modoPagamento.RuleFor(mp => mp.CartaoDebito)
                                              .NotNull()
                                              .WithMessage("Modo de pagamento é obrigatório.");
                                 modoPagamento.RuleFor(mp => mp.Dinheiro)
                                              .NotNull()
                                              .WithMessage("Modo de pagamento é obrigatório.");
                                 modoPagamento.RuleFor(mp => mp.Pix)
                                              .NotNull()
                                              .WithMessage("Modo de pagamento é obrigatório.");
                             });
                });
        }

        public async Task ValidateDataAsync
            (
                InserirOrcamentoCommand request,
                ClienteOrcamento entity,
                CancellationToken cancellationToken
            )
        {
            RuleFor(r => r.ClienteId)
                .Must(m => { return entity is not null; })
                .WithMessage("Cliente inválido ou serviço de clientes indisponível.");
            await this.ValidateAndThrowAsync(request, cancellationToken);
        }

        public async Task ValidateDataAsync
            (
                InserirOrcamentoCommand request,
                CarroOrcamento entity,
                CancellationToken cancellationToken
            )
        {
            RuleFor(r => r.ClienteId)
                .Must(m => { return entity is not null; })
                .WithMessage("Carro inválido ou serviço de veículos indisponível.");
            await this.ValidateAndThrowAsync(request, cancellationToken);
        }

        public async Task ValidateDataAsync
            (
                InserirOrcamentoCommand request,
                ProdutoOrcamento entity,
                CancellationToken cancellationToken
            )
        {
            RuleFor(r => r.ClienteId)
                .Must(m => { return entity is not null; })
                .WithMessage("Produto inválido ou serviço de produtos indisponível.");
            await this.ValidateAndThrowAsync(request, cancellationToken);
        }

        public async Task ValidateDataAsync
            (
                InserirOrcamentoCommand request,
                Servico entity,
                CancellationToken cancellationToken
            )
        {
            RuleFor(r => r.ClienteId)
                .Must(m => { return entity is not null; })
                .WithMessage("Serviço inválido.");
            await this.ValidateAndThrowAsync(request, cancellationToken);
        }
    }
}
