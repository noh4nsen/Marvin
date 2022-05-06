using Atm.Atendimento.Dto;
using Atm.Atendimento.Services;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System;
using System.Threading.Tasks;

namespace Atm.Atendimento.Api.Services
{
    public class ProdutoService : IProdutoService
    {
        private readonly IConfiguration _configuration;

        public ProdutoService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private async Task<string> Autenticar()
        {
            RestClient client = new RestClient(_configuration.GetValue<string>("api:autenticador"));
            RestRequest request = new RestRequest("login", Method.Put);
            request.AddBody(new { Login = "automar", Senha = "marvin-atm" });
            RestResponse<Login> result = await client.ExecuteAsync<Login>(request);
            Login login = result.Data;
            return login.Token;
        }

        public async Task<ProdutoOrcamento> GetProdutoById(Guid id)
        {
            RestClient client = new RestClient(_configuration.GetValue<string>("api:fornecedor"));
            RestRequest request = new RestRequest("produto/{id}", Method.Get).AddUrlSegment("id", id);
            string token = await Autenticar();
            request.AddHeader("Authorization", "Bearer " + token);

            RestResponse<ProdutoDto> result = await client.ExecuteAsync<ProdutoDto>(request);

            ProdutoDto produtoDto = result.Data;
            ProdutoOrcamento produto = new ProdutoOrcamento();

            if (produtoDto is null)
                return null;
            if (produtoDto.Id.Equals(Guid.Empty))
                return null;

            produto.IdExterno = produtoDto.Id;
            return produto;
        }

        public async Task<ProdutoOrcamento> PutProduto(ProdutoOrcamento produtoOrcamento)
        {
            RestClient client = new RestClient(_configuration.GetValue<string>("api:fornecedor"));
            RestRequest request = new RestRequest("produto/vender", Method.Put);
            string token = await Autenticar();
            request.AddHeader("Authorization", "Bearer " + token);
            request.AddBody(new { Id = produtoOrcamento.IdExterno, Quantidade = produtoOrcamento.Quantidade });

            RestResponse<ProdutoDto> result = await client.ExecuteAsync<ProdutoDto>(request);

            ProdutoDto produtoDto = result.Data;
            ProdutoOrcamento produto = new ProdutoOrcamento();

            if (produtoDto is null)
                return null;
            if (produtoDto.Id.Equals(Guid.Empty))
                return null;

            produto.IdExterno = produtoDto.Id;
            return produto;
        }

        private class ProdutoDto
        {
            public Guid Id { get; set; }
        }

        private class Login
        {
            public string Token { get; set; }
        }
    }
}
