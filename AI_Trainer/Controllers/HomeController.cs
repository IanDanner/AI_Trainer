using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AI_Trainer.Models;
using Microsoft.AspNetCore.Http;

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
        public IActionResult Index()
        {
            int? loggedId = HttpContext.Session.GetInt32("loggedId");
            if (loggedId == null)
            {
                return RedirectToAction("Index", "LoginReg");
            }
            User logged = _context.users.Where(we => we.Id == loggedId).SingleOrDefault();

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
