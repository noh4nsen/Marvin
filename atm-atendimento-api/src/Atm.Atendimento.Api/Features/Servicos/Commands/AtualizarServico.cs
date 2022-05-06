using Atm.Atendimento.Api.Extensions.Entities;
using Atm.Atendimento.Domain;
using Atm.Atendimento.Repositories;
using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Atm.Atendimento.Api.Features.Servicos.Commands
{
    public class AtualizarServicoCommand : IRequest<AtualizarServicoCommandResponse>
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public decimal? ValorAtual { get; set; }
    }

    public class AtualizarServicoCommandResponse
    {
        public DateTime? DataAtualizacao { get; set; }
    }

    public class AtualizarServicoCommandHandler : IRequestHandler<AtualizarServicoCommand, AtualizarServicoCommandResponse>
    {
        private readonly IRepository<Servico> _repository;
        private readonly AtualizarServicoCommandValidator _validator;

        public AtualizarServicoCommandHandler(IRepository<Servico> repository, AtualizarServicoCommandValidator validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<AtualizarServicoCommandResponse> Handle(AtualizarServicoCommand request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new ArgumentNullException("Erro ao processar requisição.");

            Servico entity = await GetServicoAsync(request, cancellationToken);
            await UpdateServicoAsync(request, entity);

            return entity.ToUpdateResponse();
        }

        public async Task UpdateServicoAsync(AtualizarServicoCommand request, Servico entity)
        {
            request.Update(entity);
            await _repository.UpdateAsync(entity);
            await _repository.SaveChangesAsync();
        }

        public async Task<Servico> GetServicoAsync(AtualizarServicoCommand request, CancellationToken cancellationToken)
        {
            Servico entity = await _repository.GetFirstAsync(s => s.Id.Equals(request.Id));
            await _validator.ValidateDataAsync(request, entity, cancellationToken);
            return entity;
        }
    }

    public class AtualizarServicoCommandValidator : AbstractValidator<AtualizarServicoCommand>
    {
        public AtualizarServicoCommandValidator()
        {
            RuleFor(r => r.Id)
                .NotEqual(Guid.Empty)
                .WithMessage("Id de serviço é obrigatório.");
            RuleFor(r => r.Nome)
                .NotEmpty()
                .WithMessage("Nome de serviço é obrigatório.");
        }

        public async Task ValidateDataAsync(AtualizarServicoCommand request, Servico entity, CancellationToken cancellationToken)
        {
            RuleFor(r => r.Id)
                .Must(m => { return entity is not null; })
                .WithMessage($"Serviço de id {request.Id} não encontrado.");
            await this.ValidateAndThrowAsync(request, cancellationToken);
        }
    }
}
