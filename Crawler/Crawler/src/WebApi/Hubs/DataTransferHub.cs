using Microsoft.AspNetCore.SignalR;

namespace WebApi.Hubs
{
    public class DataTransferHub:Hub
    {
        public async Task SendDataToConsole(int customAmount, int selectedOption, string token)
        {
            
           await Clients.AllExcept(Context.ConnectionId).SendAsync("ReceiveDataFromBlazor", customAmount, selectedOption, token);
           //await Clients.AllExcept(Context.ConnectionId).SendAsync("ReceiveDataFromBlazor", customAmount, selectedOption);
            
            
        }
    }
}
