using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using AI_Trainer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

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

            List<Game> games = _context.games.Include(person => person.Creator).Include(people => people.Players).ThenInclude(who => who.PlayerInfo).OrderBy(time => time.Created_at).ToList();

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

            HttpContext.Session.SetString("UserName", logged.FirstName);

            ViewBag.Logged = logged;
            ViewBag.Game = game;

            return View();
        }

		[HttpGet]
		[Route("Play")]
		public IActionResult Play()
		{
			int? loggedId = HttpContext.Session.GetInt32("loggedId");
			if (loggedId == null)
			{
				return RedirectToAction("Index", "LoginReg");
			}
			return View("GeneticAlgorithm");
		}
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
