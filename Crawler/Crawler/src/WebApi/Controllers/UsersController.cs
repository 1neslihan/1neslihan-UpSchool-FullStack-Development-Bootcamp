using Application.Features.Products.Commands.Add;
using Application.Features.Products.Queries.GetAll;
using Application.Features.SendEmail.Commands.Add;
using Application.Features.Users.Commands.UpdateNotifications;
using Application.Features.Users.Queries.GetAllUsers;
using Application.Features.Users.Queries.GetNotifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Authorize]
    public class UsersController : ApiControllerBase
    {
        [HttpPut]
        public async Task<IActionResult> UpdateUserPreferencesAsync(UpdateNotificationsCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpGet("Pull")]
        public async Task<IActionResult> PullUserPreferencesAsync(string id)
        {
            return Ok(await Mediator.Send(new GetNotificationsQuery(id)));
        }

        [HttpGet("AllUsers")]
        public async Task<IActionResult> AllUserAsync()
        {
            return Ok(await Mediator.Send(new GetAllUsersQuery()));
        }
    }
}
