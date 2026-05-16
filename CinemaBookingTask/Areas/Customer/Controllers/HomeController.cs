using CinemaBookingTask.DataAcess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaBookingTask.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var availableMovies = _context.Movies.Include(e => e.Category).Include(e => e.Cinema).Where(e => e.Status == "Available").ToList();
                

            var comingSoonMovies = _context.Movies
                .Include(e => e.Category)
                .Where(e => e.Status == "Coming Soon")
                .ToList();

            ViewBag.AvailableMovies = availableMovies;
            ViewBag.ComingSoonMovies = comingSoonMovies;

            return View();
        }
    }
}
