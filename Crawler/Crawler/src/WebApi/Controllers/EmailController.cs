using Application.Features.SendEmail.Commands.Add;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ApiControllerBase
    {
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync(SendEmailAddCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
    }
}
