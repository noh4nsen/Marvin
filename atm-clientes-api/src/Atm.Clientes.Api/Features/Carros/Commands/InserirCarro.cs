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
    public class InserirCarroCommand : IRequest<InserirCarroCommandResponse>
    {
        public string Placa { get; set; }
        public string Descricao { get; set; }
        public long? Quilometragem { get; set; }
        public string Modelo { get; set; }
        public string Marca { get; set; }
        public short? Ano { get; set; }
        public Guid ClienteId { get; set; }
    }

    public class InserirCarroCommandResponse
    {
        public Guid Id { get; set; }
        public DateTime DataCadastro { get; set; }
    }

    public class InserirCarroComandHandler : IRequestHandler<InserirCarroCommand, InserirCarroCommandResponse>
    {
        private readonly IRepository<Carro> _repositoryCarro;
        private readonly IRepository<Cliente> _repositoryCliente;
        private readonly InserirCarroCommandValidator _validator;

        public InserirCarroComandHandler
            (
                IRepository<Carro> repositoryCarro,
                IRepository<Cliente> repositoryCliente,
                InserirCarroCommandValidator validator
            )
        {
            _repositoryCarro = repositoryCarro;
            _repositoryCliente = repositoryCliente;
            _validator = validator;
        }

        public async Task<InserirCarroCommandResponse> Handle(InserirCarroCommand request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new ArgumentNullException("Erro ao processar requisição");

            Carro entity = request.ToDomain(await GetClienteAsync(request));
            await InsertCarroAsync(entity);

            return entity.ToInsertResponse();
        }

        private async Task InsertCarroAsync(Carro entity)
        {
            await _repositoryCarro.AddAsync(entity);
            await _repositoryCarro.SaveChangesAsync();
        }

        private async Task<Cliente> GetClienteAsync(InserirCarroCommand request)
        {
            Cliente cliente = await _repositoryCliente.GetFirstAsync(c => c.Id.Equals(request.ClienteId));
            await _validator.ValidateDataAsync(request, cliente);
            return cliente;
        }
    }

    public class InserirCarroCommandValidator : AbstractValidator<InserirCarroCommand>
    {
        public InserirCarroCommandValidator()
        {
            RuleFor(r => r.Placa)
                .NotNull()
                .NotEmpty()
                .WithMessage("Placa de veículo é obrigatória");
            RuleFor(r => r.ClienteId)
                .NotEmpty()

                .NotNull()
                .NotEqual(Guid.Empty)
                .WithMessage("Id de cliente é obrigatório");   
        }
        public async Task ValidateDataAsync(InserirCarroCommand request, Cliente entity)
        {
            RuleFor(r => r.ClienteId)
                .Must(c => { return entity is not null; })
                .WithMessage($"Cliente de id {request.ClienteId} não encontrado");
            await this.ValidateAndThrowAsync(request);
        }
    }
}
