using Microsoft.AspNetCore.SignalR;

namespace WebApi.Hubs
{
    public class DataTransferHub:Hub
    {
        public async Task SendDataToConsole(int customAmount, string selectedOption)
        {
            
           await Clients.User(Context.ConnectionId).SendAsync("ReceiveDataFromBlazor", customAmount, selectedOption);
           //await Clients.AllExcept(Context.ConnectionId).SendAsync("ReceiveDataFromBlazor", customAmount, selectedOption);
            
            
        }
    }
}
