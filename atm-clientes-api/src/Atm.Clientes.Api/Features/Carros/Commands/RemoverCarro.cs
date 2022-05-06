using Atm.Clientes.Api.Extensions.Entities;
using Atm.Clientes.Domain;
using Atm.Clientes.Repositories;
using FluentValidation;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Atm.Clientes.Api.Features.Carros.Commands
{
    public class RemoverCarroCommand : IRequest<RemoverCarroCommandResponse>
    {
        public Guid Id { get; set; }
        public Guid ClienteId { get; set; }
    }

    public class RemoverCarroCommandResponse
    {
        public Guid Id { get; set; }
    }

    public class RemoverCarroCommandHandler : IRequestHandler<RemoverCarroCommand, RemoverCarroCommandResponse>
    {
        private readonly IRepository<Carro> _repository;
        private readonly RemoverCarroCommandValidator _validator;

        public RemoverCarroCommandHandler(IRepository<Carro> repository, RemoverCarroCommandValidator validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<RemoverCarroCommandResponse> Handle(RemoverCarroCommand request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new ArgumentNullException("Erro ao processar requisição");

            Carro entity = await GetCarroAsync(request);
            await DeactivateCarroAsync(request, entity);

            return entity.ToRemoveResponse();
        }

        private async Task<Carro> GetCarroAsync(RemoverCarroCommand request)
        {
            Carro entity = await _repository.GetFirstAsync(c => c.Id.Equals(request.Id), c => c.Clientes);
            await _validator.ValidateDataAsync(request, entity);
            return entity;
        }

        private async Task DeactivateCarroAsync(RemoverCarroCommand request, Carro entity)
        {
            entity = await UnlinkCarroFromCliente(request, entity);
            await DeactivateCarroAsync(entity);
        }

        private async Task<Carro> UnlinkCarroFromCliente(RemoverCarroCommand request, Carro entity)
        {
            await _validator.ValidateDataAsync(request, entity.Clientes.Any(c => c.Id.Equals(request.ClienteId)));
            Cliente cliente = entity.Clientes.First(c => c.Id.Equals(request.ClienteId));
            await _validator.ValidateDataAsync(request, entity.Clientes.Remove(cliente));
            return entity;
        }

        private async Task DeactivateCarroAsync(Carro entity)
        {
            if (entity.Clientes.Count == 0)
                entity.Ativo = false;
            await _repository.UpdateAsync(entity);
            await _repository.SaveChangesAsync();
        }
    }

    public class RemoverCarroCommandValidator : AbstractValidator<RemoverCarroCommand>
    {
        public RemoverCarroCommandValidator()
        {
            RuleFor(c => c.Id)
                .NotEqual(Guid.Empty)
                .WithMessage("Id de veículo é obrigatório");
        }

        public async Task ValidateDataAsync(RemoverCarroCommand request, Carro entity)
        {
            RuleFor(r => r.Id)
                .Must(c => { return entity != null; })
                .WithMessage($"Veículo de id {request.Id} não encontrado");
            await this.ValidateAndThrowAsync(request);
        }

        public async Task ValidateDataAsync(RemoverCarroCommand request, bool unlink)
        {
            RuleFor(r => r.ClienteId)
                .Must(c => { return unlink == true; })
                .WithMessage($"Erro ao desvincular veículo de id {request.Id} de cliente de id {request.ClienteId}");
            await this.ValidateAndThrowAsync(request);
        }
    }
}
