using Atm.Clientes.Api.Extensions.Entities;
using Atm.Clientes.Domain;
using Atm.Clientes.Repositories;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Atm.Clientes.Api.Features.Clientes.Queries
{
    public class SelecionarClienteByIdQuery : IRequest<SelecionarClienteByIdQueryResponse>
    {
        public Guid Id { get; set; }
    }

    public class SelecionarClienteByIdQueryResponse
    {
        public Guid Id { get; set; }
        public bool Ativo { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Cpf { get; set; }
        public string Telefone { get; set; }
        public string Endereco { get; set; }
        public string Cep { get; set; }
        public ICollection<SelecionarClienteCarroByIdQueryResponse> Carros { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAtualizacao { get; set; }
    }

    public class SelecionarClienteCarroByIdQueryResponse
    {
        public Guid Id { get; set; }
        public bool Ativo { get; set; }
        public string Placa { get; set; }
        public string Descricao { get; set; }
        public long? Quilometragem { get; set; }
        public string Modelo { get; set; }
        public string Marca { get; set; }
        public short? Ano { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAtualizacao { get; set; }
    }

    public class SelecionarClienteByIdQueryHandler : IRequestHandler<SelecionarClienteByIdQuery, SelecionarClienteByIdQueryResponse>
    {
        private readonly IRepository<Cliente> _repository;
        private readonly SelecionarClienteByIdQueryValidator _validator;

        public SelecionarClienteByIdQueryHandler
            (
                IRepository<Cliente> repository,
                SelecionarClienteByIdQueryValidator validator
            )
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<SelecionarClienteByIdQueryResponse> Handle(SelecionarClienteByIdQuery request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new ArgumentNullException("Erro ao processar requisição");

            Cliente entity = await GetAsync(request, cancellationToken);

            return entity.ToQueryResponse();
        }

        public async Task<Cliente> GetAsync(SelecionarClienteByIdQuery request, CancellationToken cancellationToken)
        {
            Cliente entity = await _repository.GetFirstAsync(c => c.Id.Equals(request.Id), c => c.Carros);
            await _validator.ValidateDataAsync(request, entity, cancellationToken);
            return entity;
        }
    }

    public class SelecionarClienteByIdQueryValidator : AbstractValidator<SelecionarClienteByIdQuery>
    {
        public SelecionarClienteByIdQueryValidator()
        {
            RuleFor(c => c.Id)
                .NotEqual(Guid.Empty)
                .WithMessage("Id de cliente inválido");
        }

        public async Task ValidateDataAsync(SelecionarClienteByIdQuery request, Cliente entity, CancellationToken cancellationToken)
        {
            RuleFor(c => c.Id)
                .Must(c => { return entity is not null; })
                .WithMessage($"Cliente de id {request.Id} não encontrado.");
            await this.ValidateAndThrowAsync(request, cancellationToken);
        }

    }
}
