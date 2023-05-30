using Domain.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebApi.Hubs;

namespace WebApi.Controllers
{
    
    public class UserLogsController : ApiControllerBase
    {
        private readonly IHubContext<UserLogHub> _userLogHubContext;

        public UserLogsController(IHubContext<UserLogHub> userLogHubContext)
        {
            _userLogHubContext=userLogHubContext;
        }

        [HttpPost]
        public async Task<IActionResult> SendLogNotificationAsync(SendLogNotificationApiDto logNotificationApiDto)
        {
            await _userLogHubContext.Clients.AllExcept(logNotificationApiDto.ConnectionId)
                .SendAsync("NewUserLogAdded", logNotificationApiDto.Log);

            return Ok();
        }
    }
}
