using System;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using AI_Trainer.Controllers;
using AI_Trainer.Models;
using System.Linq;
using System.Collections.Generic;

namespace AI_Trainer.Hubs
{
    public class GameHub : Hub
    {
        //public override async Task OnConnectedAsync()
        //{
        //    await Clients.All.SendAsync("SendAction", Context.User.Identity.Name, "joined", "0");
        //}

        //public override async Task OnDisconnectedAsync(Exception ex)
        //{
        //    await Clients.All.SendAsync("SendAction", Context.User.Identity.Name, "left", "0");
        //}

        //public HomeController homeController;
        //public GameHub(HomeController blah)
        //{
        //    homeController = blah;
        //}


        private AIContext _context;

        public GameHub(AIContext context)
        {
            _context = context;
        }

        public async Task SendMessage(string user, string message, string GameID)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message, GameID);
        }

        public async Task SendData(string user, string userId, int[] data, string GameID)
        {
            //homeController.AddOpponentData(user, data, GameID);

            int gameid = Int32.Parse(GameID);
            int userid = Int32.Parse(userId);

            List<Player> players = _context.players.Where(we => we.GameId == gameid).ToList();
            
            foreach(Player play in players)
            {
                if (play.Id != userid)
                {
                    play.OpponentMoves = data.Select(x => (byte)x).ToArray();
                    _context.SaveChanges();
                }
            }

            await Clients.Others.SendAsync("ReceiveData", data);
        }

        public async Task SendFinale(string user, string userId, int[] data, int[] preSet, string GameID)
        {
            //homeController.AddOpponentData(user, data, GameID);

            int gameid = Int32.Parse(GameID);
            int userid = Int32.Parse(userId);

            
            await Clients.Others.SendAsync("ReceiveFinale", data, preSet);
        }

        public async Task JoinGame(string user, string GameID)
        {
            await Clients.All.SendAsync("SendAction", user, "joined the game", GameID);
        }
        
    }
}