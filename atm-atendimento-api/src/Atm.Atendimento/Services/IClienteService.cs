using Atm.Atendimento.Dto;
using System;
using System.Threading.Tasks;

namespace Atm.Atendimento.Services
{
    public interface IClienteService
    {
        Task<ClienteOrcamento> GetClienteById(Guid id);
        Task<CarroOrcamento> GetCarroById(Guid Id);
    }
}
