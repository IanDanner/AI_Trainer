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
        [Route("Home")]
        public IActionResult GameSelect()
        {
            int? loggedId = HttpContext.Session.GetInt32("loggedId");
            if (loggedId == null)
            {
                return RedirectToAction("Index", "LoginReg");
            }


            User logged = _context.users.Where(we => we.Id == loggedId).SingleOrDefault();

            List<Game> games = _context.games.Include(people => people.Players).ThenInclude(who => who.PlayerInfo).OrderBy(time => time.Created_at).ToList();

            ViewBag.Logged = logged;
            ViewBag.Games = games;
            return View();
        }

        [HttpGet]
        [Route("Game")]
        public IActionResult Index()
        {
            int? loggedId = HttpContext.Session.GetInt32("loggedId");
            if (loggedId == null)
            {
                return RedirectToAction("Index", "LoginReg");
            }
            

            User logged = _context.users.Where(we => we.Id == loggedId).SingleOrDefault();

            HttpContext.Session.SetString("UserName", logged.FirstName);

            ViewBag.Logged = logged;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
