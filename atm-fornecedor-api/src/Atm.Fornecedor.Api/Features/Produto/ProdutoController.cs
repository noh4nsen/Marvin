using Atm.Fornecedor.Api.Features.Produto.Commands;
using Atm.Fornecedor.Api.Features.Produto.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Atm.Fornecedor.Api.Features.Produto
{
    [Route("produto")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProdutoController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            return Ok(await _mediator.Send(new SelecionarProdutoByIdQuery { Id = id }));
        }

        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] SelecionarProdutoFiltersQuery request)
        {
            return Ok(await _mediator.Send(request));
        }

        [HttpGet("lista-nomes")]
        public async Task<ActionResult> Get([FromQuery] SelecionarProdutoNamesQuery request)
        {
            return Ok(await _mediator.Send(request));
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] InserirProdutoCommand request)
        {
            return Created("/produto", await _mediator.Send(request));
        }

        [HttpPut("vender")]
        public async Task<ActionResult> Put([FromBody] VenderProdutoCommand request)
        {
            return Ok(await _mediator.Send(request));
        }

        [HttpPut]
        public async Task<ActionResult> Put([FromBody] AtualizarProdutoCommand request)
        {
            return Ok(await _mediator.Send(request));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            return Ok(await _mediator.Send(new RemoverProdutoCommand { Id = id }));
        }
    }
}
