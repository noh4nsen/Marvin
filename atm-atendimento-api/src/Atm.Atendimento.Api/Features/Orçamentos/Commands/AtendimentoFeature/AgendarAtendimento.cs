using Atm.Atendimento.Api.Extensions.Entities;
using Atm.Atendimento.Domain;
using Atm.Atendimento.Domain.Enum;
using Atm.Atendimento.Dto;
using Atm.Atendimento.Repositories;
using Atm.Atendimento.Services;
using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Atm.Atendimento.Api.Features.Orçamentos.Commands.AtendimentoFeature
{
    public class AgendarAtendimentoCommand : IRequest<AgendarAtendimentoCommandResponse>
    {
        public Guid Id { get; set; }
        public DateTime DataAgendamento { get; set; }
    }

    public class AgendarAtendimentoCommandResponse
    {
        public Guid Id { get; set; }
        public DateTime? DataAgendamento { get; set; }
    }

    public class AgendarAtendimentoCommandHandler : IRequestHandler<AgendarAtendimentoCommand, AgendarAtendimentoCommandResponse>
    {
        private readonly IRepository<Orcamento> _repository;
        private readonly IProdutoService _service;
        private readonly AgendarAtendimentoCommandValidator _validator;

        public AgendarAtendimentoCommandHandler
            (
                IRepository<Orcamento> repository,
                IProdutoService service,
                AgendarAtendimentoCommandValidator validator
            )
        {
            _repository = repository;
            _service = service;
            _validator = validator;
        }

        public async Task<AgendarAtendimentoCommandResponse> Handle(AgendarAtendimentoCommand request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new ArgumentNullException("Erro ao processar requisição.");

            Orcamento entity = await GetOrcamentoAsync(request, cancellationToken);
            if (entity.DataAgendamento is null)
                await VenderProduto(request, entity, cancellationToken);
            await AgendarOrcamentoAsync(request, entity, cancellationToken);

            return entity.ToAgendarResponse();
        }

        private async Task AgendarOrcamentoAsync(AgendarAtendimentoCommand request, Orcamento entity, CancellationToken cancellationToken)
        {
            request.ToAgendamento(entity);
            await _repository.UpdateAsync(entity);
            await _repository.SaveChangesAsync();
        }

        private async Task VenderProduto
            (
                AgendarAtendimentoCommand request,
                Orcamento entity,
                CancellationToken cancellationToken
            )
        {
            foreach (var produto in entity.Produtos)
            {
                await VenderProduto(request, produto, cancellationToken);
            }
        }

        private async Task VenderProduto
            (
                AgendarAtendimentoCommand request,
                ProdutoOrcamento produto,
                CancellationToken cancellationToken
            )
        {
            await _validator.ValidateDataAsync(request, await _service.PutProduto(produto), cancellationToken);
        }

        private async Task<Orcamento> GetOrcamentoAsync(AgendarAtendimentoCommand request, CancellationToken cancellationToken)
        {
            Orcamento entity = await _repository.GetFirstAsync(o => o.Id.Equals(request.Id), o => o.Produtos);
            await _validator.ValidateDataAsync(request, entity, cancellationToken);
            await _validator.ValidateDataAsync(request, entity.Status, cancellationToken);
            return entity;
        }
    }

    public class AgendarAtendimentoCommandValidator : AbstractValidator<AgendarAtendimentoCommand>
    {
        public AgendarAtendimentoCommandValidator()
        {
            RuleFor(r => r.Id)
                .NotEqual(Guid.Empty)
                .WithMessage("Id de orçamento é obrigatório");
            RuleFor(r => r.DataAgendamento)
                .NotEmpty()
                .WithMessage("Data de agendamento é obrigatório.");
        }

        public async Task ValidateDataAsync
            (
                AgendarAtendimentoCommand request,
                Orcamento entity,
                CancellationToken cancellationToken
            )
        {
            RuleFor(r => r.Id)
                .Must(m => { return entity is not null; })
                .WithMessage($"Orçamento de id {request.Id} não encontrado.");
            await this.ValidateAndThrowAsync(request, cancellationToken);
        }

        public async Task ValidateDataAsync
            (
                AgendarAtendimentoCommand request,
                StatusEnum status,
                CancellationToken cancellationToken
            )
        {
            RuleFor(r => r.Id)
                .Must(m => { return status == StatusEnum.Cadastrado || status == StatusEnum.Agendado; })
                .WithMessage($"Orçamento de id {request.Id} já foi finalizado.");
            await this.ValidateAndThrowAsync(request, cancellationToken);
        }

        public async Task ValidateDataAsync
            (
                AgendarAtendimentoCommand request,
                ProdutoOrcamento entity,
                CancellationToken cancellationToken
            )
        {
            RuleFor(r => r.Id)
                .Must(m => { return entity is not null; })
                .WithMessage($"Erro ao processar venda de produto {entity.IdExterno} no orçamento {request.Id}.");
            await this.ValidateAndThrowAsync(request, cancellationToken);
        }
    }
}
