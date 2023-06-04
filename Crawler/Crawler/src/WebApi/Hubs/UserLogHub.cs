using Domain.Dtos;
using Microsoft.AspNetCore.SignalR;

namespace WebApi.Hubs
{
    public class UserLogHub:Hub
    {
        public async Task SendLogNotificationAsync(UserLogDto log)
        {
            await Clients.AllExcept(Context.ConnectionId).SendAsync("NewUserLogAdded", log);
        }
        
    }
}
