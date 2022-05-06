using Atm.Atendimento.Dto;
using Atm.Atendimento.Services;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System;
using System.Threading.Tasks;

namespace Atm.Atendimento.Api.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IConfiguration _configuration;

        public ClienteService(IConfiguration configuration)
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

        public async Task<CarroOrcamento> GetCarroById(Guid id)
        {
            RestClient client = new RestClient(_configuration.GetValue<string>("api:cliente"));
            RestRequest request = new RestRequest("carro/{id}", Method.Get).AddUrlSegment("id", id);
            string token = await Autenticar();
            request.AddHeader("Authorization", "Bearer " + token);

            RestResponse<CarroDto> result = await client.ExecuteAsync<CarroDto>(request);

            CarroDto carroDto = result.Data;
            CarroOrcamento carro = new CarroOrcamento();

            if (carroDto is null)
                return null;
            if (carroDto.Id.Equals(Guid.Empty))
                return null;

            carro.IdExterno = carroDto.Id;
            return carro;
        }

        public async Task<ClienteOrcamento> GetClienteById(Guid id)
        {
            RestClient client = new RestClient(_configuration.GetValue<string>("api:cliente"));
            RestRequest request = new RestRequest("cliente/{id}", Method.Get).AddUrlSegment("id", id);
            string token = await Autenticar();
            request.AddHeader("Authorization", "Bearer " + token);

            RestResponse<ClienteDto> result = await client.ExecuteAsync<ClienteDto>(request);

            ClienteDto clienteDto = result.Data;
            ClienteOrcamento cliente = new ClienteOrcamento();

            if (clienteDto is null)
                return null;
            if (clienteDto.Id.Equals(Guid.Empty))
                return null;

            cliente.IdExterno = clienteDto.Id;
            return cliente;
        }

        private class CarroDto
        {
            public Guid Id { get; set; }
        }

        private class ClienteDto
        {
            public Guid Id { get; set; }
        }

        private class Login
        {
            public string Token { get; set; }
        }
    }
}
