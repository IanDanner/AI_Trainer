//using AI_Trainer.Hubs;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace AI_Trainer.Models
//{
//    public class GamesCenter
//    {

//        private AIContext _context;

//        public GamesCenter(AIContext context)
//        {
//            _context = context;
//        }

//        public void AddGame(string game)
//        {
//            _context.games.Add(new Game
//            {
//                Name = game
//            });
//            _context.SaveChanges();
//        }

//        public bool GameExists(string game)
//        {
//            var item = _context.games.FirstOrDefault(t => t.Name == game);
//            if (item == null)
//            {
//                return false;
//            }

//            return true;
//        }

//        public void CreateNewItem(Player item)
//        {
//            if (GameExists(item.ActiveGame.Name))
//            {
//                _context.players.Add(new Player
//                {
//                    PlayerName = item.PlayerName,
//                    AIName = item.AIName,
//                    GameID = item.ActiveGame.Id,
//                    ActiveGame = item.ActiveGame
//                });
//                _context.SaveChanges();
//            }
//            else
//            {
//                throw new System.Exception("Game does not exist");
//            }
//        }

//        public IEnumerable<Player> GetAllPlayers(string game)
//        {
//            return _context.players.Where(item => item.ActiveGame.Name == game).Select(z =>
//                new Player
//                {
//                    PlayerName = z.PlayerName,
//                    AIName = z.AIName,
//                    GameID = z.GameID,
//                    ActiveGame = z.ActiveGame
//                });
//        }

//        public List<string> GetAllGames()
//        {
//            return _context.games.Select(t => t.Name).ToList();
//        }

//    }
//}
