using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using AI_Trainer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;

namespace AI_Trainer.Controllers
{
    public class HomeController : Controller
    {
        private AIContext _context;

        public HomeController(AIContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("GameSelect")]
        public IActionResult GameSelect()
        {
            int? loggedId = HttpContext.Session.GetInt32("loggedId");
            if (loggedId == null)
            {
                return RedirectToAction("Index", "LoginReg");
            }


            User logged = _context.users.Where(we => we.Id == loggedId).SingleOrDefault();

            List<Game> games = _context.games.Include(person => person.User).Include(people => people.Players).ThenInclude(who => who.User).OrderBy(time => time.Created_at).ToList();

            ViewBag.Logged = logged;
            ViewBag.Games = games;
            return View();
        }

        [HttpGet]
        [Route("Game/{gameId}")]
        public IActionResult Index(int gameId)
        {
            int? loggedId = HttpContext.Session.GetInt32("loggedId");
            if (loggedId == null)
            {
                return RedirectToAction("Index", "LoginReg");
            }
            

            User logged = _context.users.Where(we => we.Id == loggedId).SingleOrDefault();
            Game game = _context.games.Where(we => we.Id == gameId).SingleOrDefault();
            Player existing = _context.players.Where(we => we.UserId == loggedId && we.GameId == gameId).SingleOrDefault();

            if(existing == null)
            {
                Player newPlayer = new Player
                {
                    UserId = logged.Id,
                    User = logged,
                    Username = logged.FirstName,
                    AIName = "skynet",
                    GameId = gameId,
                    Game = game,
                };

                _context.players.Add(newPlayer);
                _context.SaveChanges();
            }

            ViewBag.Logged = logged;
            ViewBag.Game = game;

            return View("GeneticAlgorithm");
        }

        [HttpGet]
        [HttpPost]
        [Route("CreateGame")]
        public IActionResult CreateGame(string GameName)
        {
            int? loggedId = HttpContext.Session.GetInt32("loggedId");
            if (loggedId == null)
            {
                return RedirectToAction("Index", "LoginReg");
            }

            User logged = _context.users.Where(we => we.Id == loggedId).SingleOrDefault();

            Game newGame = new Game
            {
                Name = GameName,
                UserId = logged.Id,
                User = logged,
            };

            _context.games.Add(newGame);
            _context.SaveChanges();
            
            return RedirectToAction("Index", new { gameId = newGame.Id });
        }
        
        public void AddOpponentData(string playername, int[] data, string GameID)
        {
            int? loggedId = HttpContext.Session.GetInt32("loggedId");

            int gameid = Int32.Parse(GameID);

            Player existing = _context.players.Where(we => we.UserId == loggedId && we.GameId == gameid).SingleOrDefault();
            User logged = _context.users.Where(we => we.Id == loggedId).SingleOrDefault();


            if(logged.FirstName != playername)
            {
                existing.OpponentMoves = data.Select(x=>(byte)x).ToArray();
                _context.SaveChanges();

            }
            
            return;
        }

        //[HttpGet]
        //[Route("Play")]
        //public IActionResult Play()
        //{
        //	int? loggedId = HttpContext.Session.GetInt32("loggedId");
        //	if (loggedId == null)
        //	{
        //		return RedirectToAction("Index", "LoginReg");
        //	}
        //	return View("GeneticAlgorithm");
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
