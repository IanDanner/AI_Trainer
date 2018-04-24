using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AI_Trainer.Models;
using Newtonsoft.Json;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;

namespace C_SharpBelt.Controllers
{
    public class LoginRegController : Controller
    {
        private AIContext _context;

        public LoginRegController(AIContext context)
        {
            _context = context;
        }

        [Route("/")]
        public IActionResult Index() => View();

        [Route("register")]
        public IActionResult Register(UserRegister user)
        {
            if (ModelState.IsValid)
            {
                User logged = _context.users.SingleOrDefault(we => we.Email == user.Email);

                if (logged != null)
                {
                    TempData["Rerror"] = "Email already registered";
                    return View("Index", user);
                }

                if (!(user.Password.Any(c => char.IsLetter(c)) && user.Password.Any(c => char.IsDigit(c)) && user.Password.Any(c => char.IsSymbol(c))))
                {
                    TempData["Rerror"] = "Password must contain 1 number, 1 letter, and a special character";
                    return View("Index", user);
                }

                string hashed = HashedPass(user.Password);

                User newUser = new User
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Password = hashed,
                };
                _context.users.Add(newUser);
                _context.SaveChanges();

                logged = _context.users.SingleOrDefault(we => we.Email == user.Email);
                int userId = logged.Id;
                HttpContext.Session.SetInt32("loggedId", userId);

                return RedirectToAction("Index", "Home");
            }
            return View("Index", user);
        }

        [HttpGet]
        [HttpPost]
        [Route("login")]
        public IActionResult Login(string email, string password)
        {
            if (email == null)
            {
                TempData["error"] = "No email input";
                return RedirectToAction("Index");
            }

            User logged = _context.users.SingleOrDefault(we => we.Email == email);

            if (logged == null)
            {
                TempData["error"] = "Email not registered";
                return RedirectToAction("Index");
            }

            //TESTING LOGIN PASSWORD TO DATABASE PASSWORD
            /* Fetch the stored value */
            string savedPasswordHash = (string)logged.Password;
            /* Extract the bytes */
            byte[] hashBytes = Convert.FromBase64String(savedPasswordHash);
            /* Get the salt */
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            /* Compute the hash on the password the user entered */
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);
            /* Compare the results */
            for (int i = 0; i < 20; i++)
            {
                if (hashBytes[i + 16] != hash[i])
                {
                    TempData["error"] = "Password is Incorrect";
                    return RedirectToAction("Index");
                }
            }

            logged = _context.users.SingleOrDefault(we => we.Email == email);
            int userId = logged.Id;
            HttpContext.Session.SetInt32("loggedId", userId);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        public string HashedPass(string password)
        { //HASHING PASSWORD FOR DATABASE
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);
            string savedPasswordHash = Convert.ToBase64String(hashBytes);
            return savedPasswordHash;
        }
    }
}
