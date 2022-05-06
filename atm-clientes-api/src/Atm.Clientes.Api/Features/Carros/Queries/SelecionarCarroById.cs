using Atm.Clientes.Api.Extensions.Entities;
using Atm.Clientes.Domain;
using Atm.Clientes.Repositories;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Atm.Clientes.Api.Features.Carros.Queries
{
    public class SelecionarCarroByIdQuery : IRequest<SelecionarCarroByIdQueryResponse>
    {
        public Guid Id { get; set; }
    }

    public class SelecionarCarroByIdQueryResponse
    {
        public Guid Id { get; set; }
        public bool Ativo { get; set; }
        public string Placa { get; set; }
        public string Descricao { get; set; }
        public long? Quilometragem { get; set; }
        public string Modelo { get; set; }
        public string Marca { get; set; }
        public short? Ano { get; set; }
        public ICollection<SelecionarCarroClienteByIdQueryResponse> Clientes { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAtualizacao { get; set; }
    }

    public class SelecionarCarroClienteByIdQueryResponse
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Cpf { get; set; }
        public string Telefone { get; set; }
        public string Endereco { get; set; }
        public string Cep { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAtualizacao { get; set; }
    }

    public class SelecionarCarroByIdQueryHandler : IRequestHandler<SelecionarCarroByIdQuery, SelecionarCarroByIdQueryResponse>
    {
        private readonly IRepository<Carro> _repository;
        private readonly SelecionarCarroByIdQueryValidator _validator;

        public SelecionarCarroByIdQueryHandler(IRepository<Carro> repository, SelecionarCarroByIdQueryValidator validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<SelecionarCarroByIdQueryResponse> Handle(SelecionarCarroByIdQuery request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new ArgumentNullException("Erro ao processar requisição");

            Carro entity = await GetCarroAsync(request);

            return entity.ToQueryResponse();
        }

        public async Task<Carro> GetCarroAsync(SelecionarCarroByIdQuery request)
        {
            Carro entity = await _repository.GetFirstAsync(c => c.Id.Equals(request.Id), c => c.Clientes);
            await _validator.ValidateDataAsync(request, entity);
            return entity;
        }
    }

    public class SelecionarCarroByIdQueryValidator : AbstractValidator<SelecionarCarroByIdQuery>
    {
        public SelecionarCarroByIdQueryValidator()
        {
            RuleFor(c => c.Id)
                .NotEqual(Guid.Empty)
                .WithMessage("Id de veículo inválido");
        }

        public async Task ValidateDataAsync(SelecionarCarroByIdQuery request, Carro entity)
        {
            RuleFor(c => c.Id)
                .Must(c => { return entity != null; })
                .WithMessage($"Cliente de id {request.Id} não encontrado");
            await this.ValidateAndThrowAsync(request);
        }
    }
}
