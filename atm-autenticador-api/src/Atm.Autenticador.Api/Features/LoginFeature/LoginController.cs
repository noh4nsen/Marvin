using Atm.Autenticador.Api.Features.LoginFeature.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Atm.Autenticador.Api.Features.LoginFeature
{
    [Route("login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LoginController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPut]
        public async Task<ActionResult> Put([FromBody] LogarCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}
