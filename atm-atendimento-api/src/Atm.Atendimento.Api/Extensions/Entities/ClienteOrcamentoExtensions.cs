using Atm.Atendimento.Api.Helpers;
using Atm.Atendimento.Dto;
using System;

namespace Atm.Atendimento.Api.Extensions.Entities
{
    public static class ClienteOrcamentoExtensions
    {
        public static ClienteOrcamento ToNew(this ClienteOrcamento entity)
        {
            return new ClienteOrcamento()
            {
                Id = Guid.NewGuid(),
                Ativo = true,
                IdExterno = entity.IdExterno,
                DataCadastro = DateHelper.GetLocalTime()
            };
        }
    }
}
