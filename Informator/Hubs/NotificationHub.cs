using Microsoft.AspNetCore.SignalR;

namespace Informator.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task Send(string message, string userName)
        {
            await Clients.All.SendAsync("Receive", message, userName);
        }
    }
}
