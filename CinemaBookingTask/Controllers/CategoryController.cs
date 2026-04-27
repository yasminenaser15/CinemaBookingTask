using CinemaBookingTask.DataAcess;
using CinemaBookingTask.Models;
using CinemaBookingTask.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CinemaBookingTask.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int page = 1, string? query = null)
        {
            var categories = _context.Categories.AsQueryable();

            if (query is not null)
                categories = categories.Where(e => e.Name.ToLower().Contains(query.ToLower().Trim()));

            int totalPages = (int)Math.Ceiling(categories.Count() / 5.0);
            categories = categories.Skip((page - 1) * 5).Take(5);

            return View(new CategoryWithRelatedVM()
            {
                Categories = categories.AsEnumerable(),
                TotalPages = totalPages,
                CurrentPage = page,
                Query = query
            });
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Category());
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            ModelState.Remove("Movies");
            if (!ModelState.IsValid)
                return View(category);

            _context.Categories.Add(category);
            _context.SaveChanges();

            TempData["success_notification"] = "Add Category Successfully";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            var category = _context.Categories.SingleOrDefault(e => e.Id == id);

            if (category is null) return NotFound();

            return View(category);
        }

        [HttpPost]
        public IActionResult Update(Category category)
        {
            ModelState.Remove("Movies");
            if (!ModelState.IsValid)
                return View(category);

            _context.Categories.Update(category);
            _context.SaveChanges();

            TempData["success_notification"] = "Update Category Successfully";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var category = _context.Categories.SingleOrDefault(e => e.Id == id);

            if (category is null) return NotFound();

            _context.Categories.Remove(category);
            _context.SaveChanges();

            TempData["success_notification"] = "Delete Category Successfully";
            return RedirectToAction(nameof(Index));
        }
    }
}