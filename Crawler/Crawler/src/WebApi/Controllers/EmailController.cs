using Application.Features.SendEmail.Commands.Add;
using Application.Features.SendEmail.Commands.OrderDetails;
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

        [HttpPost("OrderDetails")]
        public async Task<IActionResult> OrderDetailsAsync(SendOrderDetailsAddCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
    }
}
