using Atm.Clientes.Api.Features.Clientes.Commands;
using Atm.Clientes.Api.Features.Clientes.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Atm.Clientes.Api.Features.Clientes
{
    [Route("cliente")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ClienteController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            return Ok(await _mediator.Send(new SelecionarClienteByIdQuery { Id = id }));
        }

        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] SelecionarClienteFiltersQuery request)
        {
            return Ok(await _mediator.Send(request));
        }


        [HttpPost]
        public async Task<ActionResult> Post([FromBody] InserirClienteCommand request)
        {
            return Created("/cliente", await _mediator.Send(request));
        }

        [HttpPut]
        public async Task<ActionResult> Put([FromBody] AtualizarClienteCommand request)
        {
            return Ok(await _mediator.Send(request));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            return Ok(await _mediator.Send(new RemoverClienteCommand { Id = id }));
        }
    }
}
