using CinemaBookingTask.DataAcess;
using CinemaBookingTask.Models;
using CinemaBookingTask.Services;
using Microsoft.AspNetCore.Mvc;
using CinemaBookingTask.ViewModels;

namespace CinemaBookingTask.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CinemaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly CinemaService _cinemaService;

        public CinemaController(ApplicationDbContext context)
        {
            _context = context;
            _cinemaService = new();
        }

        public IActionResult Index(int page = 1, string? query = null)
        {
            var cinemas = _context.Cinemas.AsQueryable();

            if (query is not null)
                cinemas = cinemas.Where(e => e.Name.ToLower().Contains(query.ToLower().Trim()));

            int totalPages = (int)Math.Ceiling(cinemas.Count() / 5.0);
            cinemas = cinemas.Skip((page - 1) * 5).Take(5);

            return View(new CinemaWithRelatedVM()
            {
                Cinemas = cinemas.AsEnumerable(),
                TotalPages = totalPages,
                CurrentPage = page,
                Query = query
            });
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreateCinemaVM());
        }

        [HttpPost]
        public IActionResult Create(CreateCinemaVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            Cinema cinema = new()
            {
                Name = vm.Name,
                Description = vm.Description,
              
            };

            if (vm.Img is not null && vm.Img.Length > 0)
            {
                var fileName = _cinemaService.SaveImg(vm.Img);
                if (fileName is not null)
                    cinema.Img = fileName;
            }

            _context.Cinemas.Add(cinema);
            _context.SaveChanges();

            TempData["success_notification"] = "Add Cinema Successfully";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            var cinema = _context.Cinemas.SingleOrDefault(e => e.Id == id);

            if (cinema is null) return NotFound();

            return View(new UpdateCinemaVM()
            {
                Id = cinema.Id,
                Name = cinema.Name,
                Description = cinema.Description,
                CurrentImg=cinema.Img
             
            });
        }

        [HttpPost]
        public IActionResult Update(UpdateCinemaVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var cinema = _context.Cinemas.SingleOrDefault(e => e.Id == vm.Id);

            if (cinema is null) return NotFound();

            cinema.Name = vm.Name;
            cinema.Description = vm.Description;
          

            if (vm.Img is not null && vm.Img.Length > 0)
            {
                _cinemaService.RemoveImg(cinema.Img);
                var fileName = _cinemaService.SaveImg(vm.Img);
                if (fileName is not null) cinema.Img = fileName;
            }

            _context.Cinemas.Update(cinema);
            _context.SaveChanges();

            TempData["success_notification"] = "Update Cinema Successfully";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var cinema = _context.Cinemas.SingleOrDefault(e => e.Id == id);

            if (cinema is null) return NotFound();

            _cinemaService.RemoveImg(cinema.Img);

            _context.Cinemas.Remove(cinema);
            _context.SaveChanges();

            TempData["success_notification"] = "Delete Cinema Successfully";
            return RedirectToAction(nameof(Index));
        }
    }
    
    }

