using Atm.Clientes.Api.Features.Carros.Commands;
using Atm.Clientes.Api.Features.Carros.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Atm.Clientes.Api.Features.Carros
{
    [Route("carro")]
    [ApiController]
    public class CarroController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CarroController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            return Ok(await _mediator.Send(new SelecionarCarroByIdQuery { Id = id }));
        }

        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] SelecionarCarroFiltersQuery request)
        {
            return Ok(await _mediator.Send(request));
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] InserirCarroCommand request)
        {
            return Created("/carro", await _mediator.Send(request));
        }

        [HttpPut]
        public async Task<ActionResult> Put([FromBody] AtualizarCarroCommand request)
        {
            return Ok(await _mediator.Send(request));
        }

        [HttpDelete("{id}/{clienteId}")]
        public async Task<ActionResult> Delete(Guid id, Guid clienteId)
        {
            return Ok(await _mediator.Send(new RemoverCarroCommand { Id = id, ClienteId = clienteId }));
        }
    }
}
