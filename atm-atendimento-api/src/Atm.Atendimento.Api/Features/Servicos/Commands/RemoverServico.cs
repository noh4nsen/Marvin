using Atm.Atendimento.Api.Extensions.Entities;
using Atm.Atendimento.Domain;
using Atm.Atendimento.Repositories;
using FluentValidation;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Atm.Atendimento.Api.Features.Servicos.Commands
{
    public class RemoverServicoCommand : IRequest<RemoverServicoCommandResponse>
    {
        public Guid Id { get; set; }
    }

    public class RemoverServicoCommandResponse
    {
        public Guid Id { get; set; }
    }

    public class RemoverServicoCommandHandler : IRequestHandler<RemoverServicoCommand, RemoverServicoCommandResponse>
    {
        private readonly IRepository<Servico> _repository;
        private readonly RemoverServicoCommandValidator _validator;

        public RemoverServicoCommandHandler(IRepository<Servico> repository, RemoverServicoCommandValidator validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<RemoverServicoCommandResponse> Handle(RemoverServicoCommand request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new ArgumentNullException("Erro ao processar requisição.");

            Servico entity = await GetServicoAsync(request, cancellationToken);
            await DeactivateServicoAsync(request, entity, cancellationToken);

            return entity.ToRemoveResponse();
        }

        private async Task DeactivateServicoAsync(RemoverServicoCommand request, Servico entity, CancellationToken cancellationToken)
        {
            await _validator.ValidateDataAsync(request, !entity.CustoServico.Any(), cancellationToken);
            await _repository.RemoveAsync(entity);
            await _repository.SaveChangesAsync();
        }

        public async Task<Servico> GetServicoAsync(RemoverServicoCommand request, CancellationToken cancellationToken)
        {
            Servico entity = await _repository.GetFirstAsync(s => s.Id.Equals(request.Id), s => s.CustoServico);
            await _validator.ValidateDataAsync(request, entity, cancellationToken);
            return entity;
        }
    }

    public class RemoverServicoCommandValidator : AbstractValidator<RemoverServicoCommand>
    {
        public RemoverServicoCommandValidator()
        {
            RuleFor(r => r.Id)
                .NotEqual(Guid.Empty)
                .WithMessage("Id de serviço é obrigatório.");
        }

        public async Task ValidateDataAsync(RemoverServicoCommand request, Servico entity, CancellationToken cancellationToken)
        {
            RuleFor(r => r.Id)
                .Must(m => { return entity is not null; })
                .WithMessage($"Serviço de id {request.Id} não encontrado.");
            await this.ValidateAndThrowAsync(request, cancellationToken);
        }

        public async Task ValidateDataAsync(RemoverServicoCommand request, bool canRemove, CancellationToken cancellationToken)
        {
            RuleFor(r => r.Id)
                .Must(m => { return canRemove is true; })
                .WithMessage($"Serviço possui Orçamento vínculado e não pode ser removido.");
            await this.ValidateAndThrowAsync(request, cancellationToken);
        }
    }
}
