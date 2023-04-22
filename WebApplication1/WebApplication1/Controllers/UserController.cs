using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Data;
using bcrypt=BCrypt.Net.BCrypt;
using System;

namespace WebApplication1.Controllers
{
    public class UserController : Controller
    {
        private readonly ProjectContext _context;

        public UserController(ProjectContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoginPost(string username, string password)
        {
            if (username == "" || password == "")
            {
                return RedirectToAction("Login");
            }




            return Redirect("/");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegisterPost(string email, string username, string password, string passwordCheck)
        {
            if (email.Trim() == "" || username.Trim() == "" || password == "" || passwordCheck == "" || password != passwordCheck)
            {
                return RedirectToAction("register");
            }

            string hashedPassword = bcrypt.HashPassword(password);

            User newUser = new User { Email = email, Username = username, Password = hashedPassword };

            _context.User.Add(newUser);
            _context.SaveChanges();

            return Redirect("/");
        }
    }
}
