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
    public class AtualizarClienteCommand : IRequest<AtualizarClienteCommandResponse>
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Cpf { get; set; }
        public string Telefone { get; set; }
        public string Endereco { get; set; }
        public string Cep { get; set; }
    }

    public class AtualizarClienteCommandResponse
    {
        public DateTime DataAtualizacao { get; set; }
    }

    public class AtualizarClienteCommandHandler : IRequestHandler<AtualizarClienteCommand, AtualizarClienteCommandResponse>
    {
        private readonly IRepository<Cliente> _repository;
        private readonly AtualizarClienteCommandValidator _validator;

        public AtualizarClienteCommandHandler(IRepository<Cliente> repository, AtualizarClienteCommandValidator validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<AtualizarClienteCommandResponse> Handle(AtualizarClienteCommand request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new ArgumentNullException("Erro ao processar requisição");

            Cliente entity = await GetClienteAsync(request);
            await UpdateClienteAsync(request, entity);
            return entity.ToUpdateResponse();
        }

        private async Task<Cliente> GetClienteAsync(AtualizarClienteCommand request)
        {
            Cliente cliente = await _repository.GetFirstAsync(c => c.Id.Equals(request.Id));
            await _validator.ValidateDataAsync(request, cliente);
            return cliente;
        }

        private async Task<Cliente> UpdateClienteAsync(AtualizarClienteCommand request ,Cliente cliente)
        {
            request.Update(cliente);
            await _repository.UpdateAsync(cliente);
            await _repository.SaveChangesAsync();
            return cliente;
        }
    }

    public class AtualizarClienteCommandValidator : AbstractValidator<AtualizarClienteCommand>
    {
        public AtualizarClienteCommandValidator()
        {
            RuleFor(c => c.Id)
                .NotEqual(Guid.Empty)
                .WithMessage("Id de cliente é obrigatório");
            RuleFor(c => c.Nome)
                .NotEmpty()
                .WithMessage("Nome de cliente é obrigatório");
        }

        public async Task ValidateDataAsync(AtualizarClienteCommand request, Cliente entity)
        {
            RuleFor(r => r.Id)
                .Must(c => { return entity is not null; })
                .WithMessage($"Cliente de id {request.Id} não encontrado");
            await this.ValidateAndThrowAsync(request);
        }
    }
}
