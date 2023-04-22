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
                Console.WriteLine("INDEXXXXXX");
                User currentUser = _context.Users.Where(u => u.Username == HttpContext.Session.GetString("User")).First();

                List<Note> notes = currentUser.Notes;
                Console.WriteLine("IM AT THE NOTES");
                return View(notes);
            }

            return View();
        }

        [HttpPost]
        public IActionResult CreateNote(string title, string description)
        {
            if (HttpContext.Session.GetString("User") == null)
                return RedirectToAction("Index");

            if (title == null || title.Trim().Length == 0 || description == null || description.Trim().Length == 0)
                return RedirectToAction("Index");

            User currentUser = _context.Users.Where(u => u.Username == HttpContext.Session.GetString("User")).First();

            DateTime today = DateTime.Today;
            string date = today.ToString("dd/MM/yyyy");

            Console.WriteLine(today);

            Note note = new() { Username = currentUser, Title = title, Description = description, Date = date, Starred = false };

            _context.Notes.Add(note);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Highlight(int noteId, bool starred)
        {
            if (HttpContext.Session.GetString("User") == null)
                return RedirectToAction("Index");

            Note thisNote = _context.Notes.FirstOrDefault(u => u.Id == noteId);

            thisNote.Starred = !starred;
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}