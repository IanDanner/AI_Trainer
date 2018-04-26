using System;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using AI_Trainer.Models;

namespace AI_Trainer.Hubs
{
    public class GameHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("SendAction", Context.User.Identity.Name, "joined");
        }

        public override async Task OnDisconnectedAsync(Exception ex)
        {
            await Clients.All.SendAsync("SendAction", Context.User.Identity.Name, "left");
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        //public Task SendToGroup(string groupName, string message)
        //{
        //    return Clients.Group(groupName).SendAsync("SendGroup", $"{Context.ConnectionId}@{groupName}: {message}");
        //}

        //public async Task JoinGroup(string groupName)
        //{
        //    await Groups.AddAsync(Context.ConnectionId, groupName);

        //    await Clients.Group(groupName).SendAsync("JoinGroup", $"{Context.ConnectionId} joined {groupName}");
        //}

        //public async Task LeaveGroup(string groupName)
        //{
        //    await Groups.RemoveAsync(Context.ConnectionId, groupName);

        //    await Clients.Group(groupName).SendAsync("LeaveGroup", $"{Context.ConnectionId} left {groupName}");
        //}

        //public Task Echo(string message)
        //{
        //    return Clients.Client(Context.ConnectionId).SendAsync("Echo", $"{Context.ConnectionId}: {message}");
        //}

        //public async Task JoinGame(string gameName, string userName)
        //{

        //    await Groups.AddAsync(Context.ConnectionId, gameName);
        //    await Clients.Group(gameName).SendAsync(userName, "Joined Game");
        //}

        //public async Task LeaveGame(string gameName, string userName)
        //{

        //    await Clients.Group(gameName).SendAsync(userName, "Left Game");
        //    await Groups.RemoveAsync(Context.ConnectionId, gameName);
        //}


    }
}