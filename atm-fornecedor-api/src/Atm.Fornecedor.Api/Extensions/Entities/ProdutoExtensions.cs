using Atm.Fornecedor.Api.Features.Produto.Commands;
using Atm.Fornecedor.Api.Features.Produto.Queries;
using Atm.Fornecedor.Api.Helpers;
using Atm.Fornecedor.Domain;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Atm.Fornecedor.Api.Extensions.Entities
{
    public class ProdutoProfile : Profile
    {
        public ProdutoProfile()
        {
            CreateMap<Produto, SelecionarProdutoQueryResponse>();
            CreateMap<InserirProdutoCommand, Produto>();
        }
    }

    public static class ProdutoExtensions
    {
        public static SelecionarProdutoQueryResponse ToQueryResponse(this Produto entity, Domain.Fornecedor fornecedor)
        {
            return new SelecionarProdutoQueryResponse()
            {
                Id = entity.Id,
                Ativo = entity.Ativo,
                CodigoNCM = entity.CodigoNCM,
                Nome = entity.Nome,
                Tipo = entity.Tipo,
                Descricao = entity.Descricao,
                QuantidadeEstoque = entity.QuantidadeEstoque,
                ValorUnitario = entity.ValorAtual,
                Fornecedor = fornecedor.ToQueryFornecedorResponse(),
                DataCadastro = entity.DataCadastro,
                DataAtualizacao = entity.DataAtualizacao
            };
        }

        public static SelecionarProdutoFornecedorQueryResponse ToQueryFornecedorResponse(this Domain.Fornecedor entity)
        {
            return new SelecionarProdutoFornecedorQueryResponse()
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

        public static Produto ToDomain(this InserirProdutoCommand request)
        {
            Produto produto = new Produto()
                                    {
                                        CodigoNCM = request.CodigoNCM,
                                        Nome = request.Nome,
                                        Tipo = request.Tipo,
                                        Descricao = request.Descricao,
                                        QuantidadeEstoque = request.QuantidadeEstoque,
                                        HistoricoProduto = SetHistoricoProdutoList(new List<HistoricoProduto>(), request.ValorUnitario).ToList(),
                                        FornecedorId = request.Fornecedor.Id
                                    };
            HistoricoProduto historicoProduto = produto.HistoricoProduto.FirstOrDefault();
            produto.HistoricoProdutoAtual = historicoProduto.Id;
            produto.ValorAtual = historicoProduto.ValorUnitario;
            return produto;
        }

        private static IEnumerable<HistoricoProduto> SetHistoricoProdutoList(IList<HistoricoProduto> historicoProduto, decimal valorUnitario)
        {
            IList<HistoricoProduto> listaHistorico = historicoProduto;
            if (listaHistorico.Count == 0)
                listaHistorico.Add(GetNewHistoricoProduto(valorUnitario));
           return listaHistorico.OrderByDescending(d => d.DataCadastro);  
        }

        public static HistoricoProduto GetNewHistoricoProduto(decimal valorUnitario)
        {
            return new HistoricoProduto
            {
                Id = Guid.NewGuid(),
                ValorUnitario = valorUnitario,
                DataCadastro = DateHelper.GetLocalTime()
            };
        }

        public static InserirProdutoCommandResponse ToInsertResponse(this Domain.Produto entity)
        {
            return new InserirProdutoCommandResponse()
            {
                Id = entity.Id,
                DataCadastro = entity.DataCadastro
            };
        }

        public static void Update(this AtualizarProdutoCommand request, Produto entity)
        {
            entity.CodigoNCM = request.CodigoNCM;
            entity.Nome = request.Nome;
            entity.Tipo = request.Tipo;
            entity.Descricao = request.Descricao;
            entity.QuantidadeEstoque = request.QuantidadeEstoque;
            entity.HistoricoProduto = SetHistoricoProdutoList(entity.HistoricoProduto.ToList(), request.ValorUnitario).ToList();
            entity.HistoricoProdutoAtual = entity.HistoricoProduto.FirstOrDefault().Id;
            entity.ValorAtual = request.ValorUnitario;
            entity.FornecedorId = request.Fornecedor.Id;
        }

        public static AtualizarProdutoCommandResponse ToUpdateResponse(this Domain.Produto entity)
        {
            return new AtualizarProdutoCommandResponse()
            {
                DataAtualizacao = (DateTime)(entity.DataAtualizacao)
            };
        }

        public static RemoverProdutoCommandResponse ToRemoveResponse(this Domain.Produto entity)
        {
            return new RemoverProdutoCommandResponse()
            {
                Id = entity.Id
            };
        }

        public static IEnumerable<SelecionarProdutoQueryResponse> ToQueryResponse(this IEnumerable<Produto> produtos, IEnumerable<Domain.Fornecedor> listaFornecedores)
        {
            IList<SelecionarProdutoQueryResponse> response = new List<SelecionarProdutoQueryResponse>();
            foreach (var entity in produtos.Zip(listaFornecedores, Tuple.Create))
                response.Add(entity.Item1.ToQueryResponse(entity.Item2));
            return response;
        }

        public static IEnumerable<SelecionarProdutoNamesQueryResponse> ToNameResponse(this IEnumerable<Produto> produtos)
        {
            IList<SelecionarProdutoNamesQueryResponse> response = new List<SelecionarProdutoNamesQueryResponse>();
            foreach (var produto in produtos)
                response.Add(produto.ToNameResponse());
            return response;
        }

        public static SelecionarProdutoNamesQueryResponse ToNameResponse(this Produto entity)
        {
            return new SelecionarProdutoNamesQueryResponse()
            {
                Nome = entity.Nome
            };
        }

        public static void VenderProduto(this VenderProdutoCommand request, Produto entity)
        {
            int novaQuantidade = entity.QuantidadeEstoque - request.Quantidade;
            if (novaQuantidade >= 0)
                entity.QuantidadeEstoque = novaQuantidade;
            else
                entity.QuantidadeEstoque = 0;
        }
    }
}
