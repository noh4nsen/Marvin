using Atm.Fornecedor.Api.Features.Fornecedor.Commands;
using Atm.Fornecedor.Api.Features.Fornecedor.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Atm.Fornecedor.Api.Features.Fornecedor
{
    [Route("fornecedor")]
    [ApiController]
    public class FornecedorController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FornecedorController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            return Ok(await _mediator.Send(new SelecionarFornecedorByIdQuery { Id = id }));
        }

        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] SelecionarFornecedorFiltersQuery request)
        {
            return Ok(await _mediator.Send(request));
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] InserirFornecedorCommand request)
        {
            return Created("/fornecedor", await _mediator.Send(request));
        }

        [HttpPut]
        public async Task<ActionResult> Put([FromBody] AtualizarFornecedorCommand request)
        {
            return Ok(await _mediator.Send(request));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            return Ok(await _mediator.Send(new RemoverFornecedorCommand { Id = id }));
        }
    }
}
