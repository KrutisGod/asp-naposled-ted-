using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Data;
using System.Linq;
using Bcrypt = BCrypt.Net.BCrypt;
using System;
using WebApplication1.Models;
using Microsoft.AspNetCore.Http;

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
            if (username == null || password == null)
                return RedirectToAction("Login");

            User user = _context.User
                .Where(u => u.Username == username)
                .FirstOrDefault();

            if (user == null)
                return RedirectToAction("Login");

            if (!Bcrypt.Verify(password, user.Password))
                return RedirectToAction("Login");
            
            HttpContext.Session.SetString("User", user.Username);

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

            if (email == null || email.Trim().Length == 0 || username == null || username.Trim().Length == 0 || password == null || password != passwordCheck)
            {
                return RedirectToAction("Register");
            }

            User sameUser = _context.User
                .Where(u => u.Username == username)
                .FirstOrDefault();

            User sameEmail = _context.User
                .Where(u => u.Email == email)
                .FirstOrDefault();

            if (sameUser != null || sameEmail != null)
            {
                return RedirectToAction("Register");
            }

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            User newUser = new User { Email = email, Username = username, Password = hashedPassword };

            _context.User.Add(newUser);
            _context.SaveChanges();

            return Redirect("/");
        }
    }
}
