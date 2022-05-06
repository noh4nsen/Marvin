using Atm.Atendimento.Api.Features.Servicos.Commands;
using Atm.Atendimento.Api.Features.Servicos.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Atm.Atendimento.Api.Features.Servicos
{
    [Route("servico")]
    [ApiController]
    public class ServicoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ServicoController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            return Ok(await _mediator.Send(new SelecionarServicoByIdQuery { Id = id }));
        }

        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] SelecionarServicoFiltersQuery request)
        {
            return Ok(await _mediator.Send(request));
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] InserirServicoCommand request)
        {
            return Ok(await _mediator.Send(request));
        }

        [HttpPut]
        public async Task<ActionResult> Put([FromBody] AtualizarServicoCommand request)
        {
            return Ok(await _mediator.Send(request));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            return Ok(await _mediator.Send(new RemoverServicoCommand { Id = id }));
        }
    }
}
