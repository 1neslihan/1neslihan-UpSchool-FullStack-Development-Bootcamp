using Application.Common.Models.SignalR;
using Microsoft.AspNetCore.SignalR;

namespace WebApi.Hubs
{
    public class UserLogHub:Hub
    {
        public async Task SendLogNotificationAsync(UserLogDto log)
        {
            await Clients.AllExcept(Context.ConnectionId).SendAsync("NewUserLogAdded", log);
        }

        public async Task OrderDetailsAsync(FormattedLogDto details)
        {
            await Clients.AllExcept(Context.ConnectionId).SendAsync("OrderDetailsAdded", details);
        }
        
    }
}
