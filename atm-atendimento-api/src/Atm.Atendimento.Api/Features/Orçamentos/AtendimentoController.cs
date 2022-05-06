using Atm.Atendimento.Api.Features.Orçamentos.Commands.AtendimentoFeature;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Atm.Atendimento.Api.Features.Orçamentos
{
    [Route("atendimento")]
    [ApiController]
    public class AtendimentoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AtendimentoController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPut("agendar")]
        public async Task<ActionResult> Put([FromBody] AgendarAtendimentoCommand request)
        {
            return Ok(await _mediator.Send(request));
        }

        [HttpPut("cancelar")]
        public async Task<ActionResult> Put([FromBody] CancelarAgendamentoCommand request)
        {
            return Ok(await _mediator.Send(request));
        }

        [HttpPut("finalizar")]
        public async Task<ActionResult> Put([FromBody] FinalizarAtendimentoCommand request)
        {
            return Ok(await _mediator.Send(request));
        }

        [HttpPut("desfinalizar")]
        public async Task<ActionResult> Put([FromBody] DesfinalizarAtendimentoCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}
