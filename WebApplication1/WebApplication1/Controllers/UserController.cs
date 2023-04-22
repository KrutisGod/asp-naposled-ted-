using Microsoft.AspNetCore.Mvc;
using MaturitaPvaCviceniASP.Models;
using BCrypt;
using System;

namespace MaturitaPvaCviceniASP.Controllers
{
    public class UserController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoginPost()
        {
            return Redirect("/");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegisterPost(string username, string password, string passwordCheck)
        {
            if (username.Trim() == "" || password == "" || passwordCheck == "" || password != passwordCheck)
            {
                return RedirectToAction("register");
            }

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            User newUser = new User { Username = username, Password = hashedPassword };

            return Redirect("/");
        }
    }
}
