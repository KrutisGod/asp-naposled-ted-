using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Data;
using System.Linq;
using Bcrypt = BCrypt.Net.BCrypt;
using System;
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
            if (HttpContext.Session.GetString("User") != null)
                return Redirect("/");

            return View();
        }

        [HttpPost]
        public IActionResult LoginPost(string username, string password)
        {
            if (username == null || password == null)
                return RedirectToAction("Login");

            User user = _context.Users
                .FirstOrDefault(u => u.Username == username);

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
            if (HttpContext.Session.GetString("User") != null)
                return Redirect("/");

            return View();
        }

        [HttpPost]
        public IActionResult RegisterPost(string email, string username, string password, string passwordCheck)
        {

            if (email == null || email.Trim().Length == 0 || username == null || username.Trim().Length == 0 || password == null || password != passwordCheck)
                return RedirectToAction("Register");

            User sameUser = _context.Users
                .FirstOrDefault(u => u.Username == username);

            User sameEmail = _context.Users
                .FirstOrDefault(e => e.Email == email);

            if (sameUser != null || sameEmail != null)
                return RedirectToAction("Register");

            string hashedPassword = Bcrypt.HashPassword(password);

            User newUser = new() { Email = email, Username = username, Password = hashedPassword };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            return Redirect("/");
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/");
        }

        [HttpPost]
        public IActionResult DeleteUser(string username, string password)
        {
            if (username == null || username.Trim().Length == 0 || password == null)
                return Redirect("/");

            User user = _context.Users
                .FirstOrDefault(u => u.Username == username);

            if (user == null)
                return Redirect("/");

            if (!Bcrypt.Verify(password, user.Password))
                return Redirect("/");

            foreach (Note note in _context.Notes)
            {
                if (note.Username.Equals(username))
                {
                    _context.Notes.Remove(note);
                    _context.SaveChanges();
                }
            }

            _context.Users.Remove(user);
            _context.SaveChanges();

            HttpContext.Session.Clear();
            return Redirect("/");
        }
    }
}
