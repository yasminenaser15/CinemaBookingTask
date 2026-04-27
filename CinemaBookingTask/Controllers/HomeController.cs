using CinemaBookingTask.DataAcess;
using CinemaBookingTask.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CinemaBookingTask.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.MoviesCount = _context.Movies.Count();
            ViewBag.CinemasCount = _context.Cinemas.Count();
            ViewBag.ActorsCount = _context.Actors.Count();
            ViewBag.CategoriesCount = _context.Categories.Count();

            ViewBag.LatestMovies = _context.Movies
                                    .Include(e => e.Category)
                                    .Include(e => e.Cinema)
                                    .OrderByDescending(e => e.Id)
                                    .Take(5)
                                    .ToList();

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
