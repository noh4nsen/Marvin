using Atm.Clientes.Api.Extensions.Entities;
using Atm.Clientes.Domain;
using Atm.Clientes.Repositories;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Atm.Clientes.Api.Features.Clientes.Commands
{
    public class InserirClienteCommand : IRequest<InserirClienteCommandResponse>
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Cpf { get; set; }
        public string Telefone { get; set; }
        public string Endereco { get; set; }
        public string Cep { get; set; }

        public ICollection<InserClienteCarroCommand> Carros { get; set; }
    }

    public class InserClienteCarroCommand
    {
        public Guid? IdCarro { get; set; }
        public string Placa { get; set; }
        public string Descricao { get; set; }
        public long? Quilometragem { get; set; }
        public string Modelo { get; set; }
        public string Marca { get; set; }
        public short? Ano { get; set; }
    }

    public class InserirClienteCommandResponse
    {
        public Guid Id { get; set; }
        public DateTime DataCadastro { get; set; }
    }

    public class InserirClienteCommandHandler : IRequestHandler<InserirClienteCommand, InserirClienteCommandResponse>
    {
        private readonly IRepository<Cliente> _repositoryCliente;
        private readonly IRepository<Carro> _repositoryCarro;

        public InserirClienteCommandHandler(IRepository<Cliente> repositoryCliente, IRepository<Carro> repositoryCarro)
        {
            _repositoryCliente = repositoryCliente;
            _repositoryCarro = repositoryCarro;
        }

        public async Task<InserirClienteCommandResponse> Handle(InserirClienteCommand request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new ArgumentNullException("Erro ao processar requisição");

            Cliente entity = await InsertClienteAsync(request.ToDomain());
            return entity.ToInsertResponse();
        }

        private async Task<Cliente> InsertClienteAsync(Cliente cliente)
        {
            await UpsertCarrosAsync(cliente.Carros);
            await _repositoryCliente.AddAsync(cliente);
            await _repositoryCliente.SaveChangesAsync();
            return cliente;
        }

        private async Task<IEnumerable<Carro>> UpsertCarrosAsync(IEnumerable<Carro> carros)
        {
            foreach (Carro carro in carros)
            {
                if (carro.Id == Guid.Empty)
                    await _repositoryCarro.AddAsync(carro);
                else
                    await _repositoryCarro.UpdateAsync(carro);
                await _repositoryCarro.SaveChangesAsync();
            }
            return carros;
        }
    }

    public class InserirClienteCommandValidator : AbstractValidator<InserirClienteCommand>
    {
        public InserirClienteCommandValidator()
        {
            RuleFor(c => c.Nome).NotEmpty()
                                .WithMessage("Nome de cliente é obrigatório");

            RuleFor(c => c.Carros).NotEmpty()
                                  .WithMessage("É necessário o vínculo de ao menos um veículo com cliente")
                                  .ForEach(rule =>
                                  {
                                      rule.Must(carro => !carro.Placa.Equals(string.Empty))
                                        .WithMessage("Placa de veículo é obrigatória");
                                  });
        }
    }
}
