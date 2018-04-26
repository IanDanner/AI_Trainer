using System;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace AI_Trainer.Hubs
{
    public class GameHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("SendAction", Context.User.Identity.Name, "joined", "0");
        }

        public override async Task OnDisconnectedAsync(Exception ex)
        {
            await Clients.All.SendAsync("SendAction", Context.User.Identity.Name, "left", "0");
        }

        public async Task SendMessage(string user, string message, string GameID)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message, GameID);
        }

        public async Task JoinGame(string user, string GameID)
        {
            await Clients.All.SendAsync("SendAction", user, "joined the game", GameID);
        }
        
    }
}