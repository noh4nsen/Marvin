using Atm.Clientes.Api.Extensions.Entities;
using Atm.Clientes.Domain;
using Atm.Clientes.Repositories;
using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Atm.Clientes.Api.Features.Carros.Commands
{
    public class AtualizarCarroCommand : IRequest<AtualizarCarroCommandResponse>
    {
        public Guid Id { get; set; }
        public string Placa { get; set; }
        public string Descricao { get; set; }
        public long? Quilometragem { get; set; }
        public string Modelo { get; set; }
        public string Marca { get; set; }
        public short? Ano { get; set; }
        public Guid? ClienteId { get; set; }
    }

    public class AtualizarCarroCommandResponse
    {
        public DateTime DataAtualizacao { get; set; }
    }

    public class AtualizarCarroCommandHandler : IRequestHandler<AtualizarCarroCommand, AtualizarCarroCommandResponse>
    {
        private readonly IRepository<Carro> _repository;
        private readonly IRepository<Cliente> _repositoryCliente;
        private readonly AtualizarCarroCommandValidator _validator;

        public AtualizarCarroCommandHandler(IRepository<Carro> repository, IRepository<Cliente> repositoryCliente, AtualizarCarroCommandValidator validator)
        {
            _repository = repository;
            _repositoryCliente = repositoryCliente;
            _validator = validator;
        }

        public async Task<AtualizarCarroCommandResponse> Handle(AtualizarCarroCommand request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new ArgumentNullException("Erro ao processar requisição.");

            Carro entity = await GetCarroAsync(request, cancellationToken);
            await UpdateCarroAsync(request, entity, await GetClienteAsync(request, cancellationToken));

            return entity.ToUpdateResponse();
        }

        private async Task<Carro> GetCarroAsync(AtualizarCarroCommand request, CancellationToken cancellationToken)
        {
            Carro entity = await _repository.GetFirstAsync(c => c.Id.Equals(request.Id), c => c.Clientes);
            await _validator.ValidateDataAsync(request, entity, cancellationToken);
            return entity;
        }

        private async Task<Cliente> GetClienteAsync(AtualizarCarroCommand request, CancellationToken cancellationToken)
        {
            Cliente entity = await _repositoryCliente.GetFirstAsync(c => c.Id.Equals(request.ClienteId));
            await _validator.ValidateDataAsync(request, entity, cancellationToken);
            return entity;
        }

        private async Task UpdateCarroAsync(AtualizarCarroCommand request, Carro carro, Cliente cliente)
        {
            request.Update(carro, cliente);
            await _repository.UpdateAsync(carro);
            await _repository.SaveChangesAsync();
        }
    }

    public class AtualizarCarroCommandValidator : AbstractValidator<AtualizarCarroCommand>
    {
        public AtualizarCarroCommandValidator()
        {
            RuleFor(r => r.Id)
                .NotEqual(Guid.Empty)
                .WithMessage("Id do veículo é obrigatório");
            RuleFor(r => r.Placa)
                .NotEmpty()
                .WithMessage("Placa de veículo é obrigatório");
        }

        public async Task ValidateDataAsync(AtualizarCarroCommand request, Carro entity, CancellationToken cancellationToken)
        {
            RuleFor(r => r.Id)
                .Must(c => { return entity is not null; })
                .WithMessage($"Veículo de id {request.Id} não encontrado.");
            await this.ValidateAndThrowAsync(request, cancellationToken);
        }

        public async Task ValidateDataAsync(AtualizarCarroCommand request, Cliente entity, CancellationToken cancellationToken)
        {
            RuleFor(r => r.ClienteId)
                .Must(c => { return entity is not null; })
                .WithMessage($"Cliente de id {request.ClienteId} não encontrado.");
            await this.ValidateAndThrowAsync(request, cancellationToken);
        }
    }
}
