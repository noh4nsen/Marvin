using Atm.Fornecedor.Api.Features.Fornecedor.Commands;
using Atm.Fornecedor.Api.Features.Fornecedor.Queries;
using System;
using System.Collections.Generic;

namespace Atm.Fornecedor.Api.Extensions.Entities
{
    public static class FornecedorExtensions
    {
        public static SelecionarFornecedorQueryResponse ToQueryResponse(this Domain.Fornecedor entity)
        {
            return new SelecionarFornecedorQueryResponse()
            {
                Id = entity.Id,
                Nome = entity.Nome,
                Cnpj = entity.Cnpj,
                Telefone = entity.Telefone,
                Email = entity.Email,
                Tipo = entity.Tipo,
                Endereco = entity.Endereco,
                DataCadastro = entity.DataCadastro,
                DataAtualizacao = entity.DataAtualizacao
            };
        }

        public static Domain.Fornecedor ToDomain(this InserirFornecedorCommand request)
        {
            return new Domain.Fornecedor()
            {
                Nome = request.Nome,
                Cnpj = request.Cnpj,
                Telefone = request.Telefone,
                Email = request.Email,
                Tipo = request.Tipo,
                Endereco = request.Endereco
            };
        }

        public static InserirFornecedorCommandResponse ToInsertResponse(this Domain.Fornecedor entity)
        {
            return new InserirFornecedorCommandResponse()
            {
                Id = entity.Id,
                DataCadastro = entity.DataCadastro
            };
        }

        public static void Update(this AtualizarFornecedorCommand request, Domain.Fornecedor entity)
        {
            entity.Nome = request.Nome;
            entity.Cnpj = request.Cnpj;
            entity.Telefone = request.Telefone;
            entity.Email = request.Email;
            entity.Tipo = request.Tipo;
            entity.Endereco = request.Endereco;
        }

        public static AtualizarFornecedorCommandResponse ToUpdateResponse(this Domain.Fornecedor entity)
        {
            return new AtualizarFornecedorCommandResponse()
            {
                DataAtualizacao = (DateTime)(entity.DataAtualizacao)
            };
        }

        public static RemoverFornecedorCommandResponse ToRemoveResponse(this Domain.Fornecedor entity)
        {
            return new RemoverFornecedorCommandResponse()
            {
                Id = entity.Id
            };
        }

        public static IEnumerable<SelecionarFornecedorQueryResponse> ToQueryResponse(this IEnumerable<Domain.Fornecedor> list)
        {
            IList<SelecionarFornecedorQueryResponse> response = new List<SelecionarFornecedorQueryResponse>();
            foreach (var entity in list)
                response.Add(entity.ToQueryResponse());
            return response;
        }
    }
}
