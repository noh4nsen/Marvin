using Atm.Atendimento.Api.Helpers;
using Atm.Atendimento.Dto;
using System;

namespace Atm.Atendimento.Api.Extensions.Entities
{
    public static class CarroOrcamentoExtensions
    {
        public static CarroOrcamento ToNew(this CarroOrcamento entity)
        {
            return new CarroOrcamento()
            {
                Id = Guid.NewGuid(),
                Ativo = true,
                IdExterno = entity.IdExterno,
                DataCadastro = DateHelper.GetLocalTime()
            };
        }
    }
}
