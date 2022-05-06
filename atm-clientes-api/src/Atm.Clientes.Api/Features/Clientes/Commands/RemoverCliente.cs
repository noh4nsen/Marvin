using Atm.Clientes.Api.Extensions.Entities;
using Atm.Clientes.Domain;
using Atm.Clientes.Repositories;
using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Atm.Clientes.Api.Features.Clientes.Commands
{
    public class RemoverClienteCommand : IRequest<RemoverClienteCommandResponse>
    {
        public Guid Id { get; set; }
    }

    public class RemoverClienteCommandResponse
    {
        public Guid Id { get; set; }
    }

    public class RemoverClienteCommandHandler : IRequestHandler<RemoverClienteCommand, RemoverClienteCommandResponse>
    {
        private readonly IRepository<Cliente> _repository;
        private readonly RemoverClienteCommandValidator _validator;

        public RemoverClienteCommandHandler
            (
                IRepository<Cliente> repositoryCliente, 
                RemoverClienteCommandValidator validator
            )
        {
            _repository = repositoryCliente;
            _validator = validator;
        }

        public async Task<RemoverClienteCommandResponse> Handle(RemoverClienteCommand request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new ArgumentNullException("Erro ao processar requisição");

            Cliente entity = await GetClienteAsync(request);
            await RemoveClienteAsync(entity);

            return entity.ToRemoveResponse();
        }

        private async Task<Cliente> GetClienteAsync(RemoverClienteCommand request)
        {
            Cliente entity = await _repository.GetFirstAsync(c => c.Id.Equals(request.Id));
            await _validator.ValidateDataAsync(request, entity);
            return entity;
        }

        private async Task RemoveClienteAsync(Cliente entity)
        {
            entity.Ativo = false;
            await _repository.UpdateAsync(entity);
            await _repository.SaveChangesAsync();
        }
    }

    public class RemoverClienteCommandValidator : AbstractValidator<RemoverClienteCommand>
    {
        public RemoverClienteCommandValidator()
        {
            RuleFor(c => c.Id)
                .NotEqual(Guid.Empty)
                .WithMessage("Id de cliente é obrigatório");
        }

        public async Task ValidateDataAsync(RemoverClienteCommand request, Cliente entity)
        {
            RuleFor(r => r.Id)
               .Must(f => { return entity != null; })
               .WithMessage($"Cliente de id {request.Id} não encontrado.");
            await this.ValidateAndThrowAsync(request);
        }
    }
}
