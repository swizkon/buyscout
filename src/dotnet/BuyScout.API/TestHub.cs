using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace BuyScout.API
{
    public class TestHub : Hub<ITestHubClient>
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.ReceiveMessage(user, message);
        }

        public async Task Broadcast(string user, string message)
        {
            await Clients.All.Broadcast(user, message);
        }

        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "All Connected Users");
            await base.OnConnectedAsync();
        }
    }
}