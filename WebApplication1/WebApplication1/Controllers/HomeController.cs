using WebApplication1.Data;
using WebApplication1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Drawing;
using Microsoft.AspNetCore.Http;
using WebApplication1.Migrations;


namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ProjectContext _context;

        public HomeController(ProjectContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("User") != null)
            {
                User currentUser = _context.Users
                    .First(u => u.Username == HttpContext.Session.GetString("User"));
                List<Note> notes = currentUser.Notes;

                return View(notes);
            }

            return View();
        }

        [HttpPost]
        public IActionResult CreateNote(string title, string description)
        {
            if (HttpContext.Session.GetString("User") == null)
                return Redirect("/");

            if (title == null || title.Trim().Length == 0 || description == null || description.Trim().Length == 0)
                return Redirect("/");

            User currentUser = _context.Users
                .First(u => u.Username == HttpContext.Session.GetString("User"));

            DateTime today = DateTime.Today;
            string date = today.ToString("dd/MM/yyyy");

            Note note = new() { Username = currentUser, Title = title, Description = description, Date = date, Starred = false };

            _context.Notes.Add(note);
            _context.SaveChanges();

            return Redirect("/");
        }

        [HttpGet]
        public IActionResult Highlight(int noteId, bool starred)
        {
            if (HttpContext.Session.GetString("User") == null)
                return Redirect("/");

            Note thisNote = _context.Notes
                .FirstOrDefault(u => u.Id == noteId);

            thisNote.Starred = !starred;
            _context.SaveChanges();

            return Redirect("/");
        }

        [HttpGet]
        public IActionResult DeleteNote(int noteId)
        {
            if (HttpContext.Session.GetString("User") == null)
                return Redirect("/");

            Note note = _context.Notes
                .FirstOrDefault(u => u.Id == noteId);

            _context.Notes.Remove(note);
            _context.SaveChanges();

            return Redirect("/");
        }
    }
}