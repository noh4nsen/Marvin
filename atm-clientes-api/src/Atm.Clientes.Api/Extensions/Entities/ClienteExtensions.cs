using Atm.Clientes.Api.Features.Clientes.Commands;
using Atm.Clientes.Api.Features.Clientes.Queries;
using Atm.Clientes.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Atm.Clientes.Api.Extensions.Entities
{
    public static class ClienteExtensions
    {
        public static Cliente ToDomain(this InserirClienteCommand request)
        {
            return new Cliente()
            {
                Ativo = true,
                Nome = request.Nome,
                Email = request.Email,
                Cpf = request.Cpf,
                Telefone = request.Telefone,
                Endereco = request.Endereco,
                Cep = request.Cep,
                Carros = request.CarrosToDomain()
            };
        }

        private static ICollection<Carro> CarrosToDomain(this InserirClienteCommand request)
        {
            if (request.Carros.Count <= 0)
                return new List<Carro>();

            IList<Carro> carros = new List<Carro>();
            foreach (InserClienteCarroCommand carro in request.Carros)
            {
                carros.Add(new Carro
                {
                    Ativo = true,
                    Id = carro.IdCarro ?? Guid.Empty,
                    Placa = carro.Placa,
                    Descricao = carro.Descricao,
                    Quilometragem = carro.Quilometragem,
                    Marca = carro.Marca,
                    Modelo = carro.Modelo,
                    Ano = carro.Ano
                });
            }

            return carros;
        }

        public static InserirClienteCommandResponse ToInsertResponse(this Cliente entity)
        {
            return new InserirClienteCommandResponse()
            {
                Id = entity.Id,
                DataCadastro = entity.DataCadastro
            };
        }

        public static SelecionarClienteByIdQueryResponse ToQueryResponse(this Cliente entity)
        {
            entity.Carros.ToList().ForEach(c => c.Clientes = null);
            return new SelecionarClienteByIdQueryResponse()
            {
                Id = entity.Id,
                Ativo = entity.Ativo,
                Nome = entity.Nome,
                Email = entity.Email,
                Cpf = entity.Cpf,
                Telefone = entity.Telefone,
                Endereco = entity.Endereco,
                Cep = entity.Cep,
                DataCadastro = entity.DataCadastro,
                DataAtualizacao = entity.DataAtualizacao,
                Carros = entity.Carros.ToQueryCarroResponse().ToList()
            };
        }

        public static IEnumerable<SelecionarClienteCarroByIdQueryResponse> ToQueryCarroResponse(this IEnumerable<Carro> list)
        {
            if (!list.Any())
                return new List<SelecionarClienteCarroByIdQueryResponse>();

            IList<SelecionarClienteCarroByIdQueryResponse> response = new List<SelecionarClienteCarroByIdQueryResponse>();
            foreach (Carro carro in list)
                response.Add(carro.ToQueryCarroResponse());
            return response;
        }

        public static SelecionarClienteCarroByIdQueryResponse ToQueryCarroResponse(this Carro entity)
        {
            return new SelecionarClienteCarroByIdQueryResponse()
            {
                Id = entity.Id,
                Placa = entity.Placa,
                Descricao = entity.Descricao,
                Quilometragem = entity.Quilometragem,
                Modelo = entity.Modelo,
                Marca = entity.Marca,
                Ano = entity.Ano,
                Ativo = entity.Ativo,
                DataCadastro = entity.DataCadastro,
                DataAtualizacao = entity.DataAtualizacao
            };
        }

        public static RemoverClienteCommandResponse ToRemoveResponse(this Cliente entity)
        {
            return new RemoverClienteCommandResponse()
            {
                Id = entity.Id
            };
        }

        public static AtualizarClienteCommandResponse ToUpdateResponse(this Cliente entity)
        {
            return new AtualizarClienteCommandResponse()
            {
                DataAtualizacao = (DateTime) entity.DataAtualizacao
            };
        }

        public static void Update(this AtualizarClienteCommand request, Cliente entity)
        {
            entity.Nome = request.Nome;
            entity.Ativo = true;
            entity.Email = request.Email;
            entity.Cpf = request.Cpf;
            entity.Telefone = request.Telefone;
            entity.Endereco = request.Endereco;
            entity.Cep = request.Cep;
        }

        public static IEnumerable<SelecionarClienteByIdQueryResponse> ToFiltersQueryResponse(this IEnumerable<Cliente> list)
        {
            IList<SelecionarClienteByIdQueryResponse> response = new List<SelecionarClienteByIdQueryResponse>();
            foreach (Cliente entity in list)
                response.Add(entity.ToQueryResponse());
            return response;
        }
    }
}
