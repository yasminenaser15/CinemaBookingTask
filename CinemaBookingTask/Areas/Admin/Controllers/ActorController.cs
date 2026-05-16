using CinemaBookingTask.DataAcess;
using CinemaBookingTask.Models;
using CinemaBookingTask.Services;
using CinemaBookingTask.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CinemaBookingTask.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ActorController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ActorService _actorService;

        public ActorController(ApplicationDbContext context)
        {
            _context = context;
            _actorService = new();
        }

        public IActionResult Index(int page = 1, string? query = null)
        {
            var actors = _context.Actors.AsQueryable();

            if (query is not null)
                actors = actors.Where(e => e.Name.ToLower().Contains(query.ToLower().Trim()));

            int totalPages = (int)Math.Ceiling(actors.Count() / 5.0);
            actors = actors.Skip((page - 1) * 5).Take(5);

            return View(new ActorWithRelatedVM()
            {
                Actors = actors.AsEnumerable(),
                TotalPages = totalPages,
                CurrentPage = page,
                Query = query
            });
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreateActorVM());
        }

        [HttpPost]
        public IActionResult Create(CreateActorVM vm)
        {
            ModelState.Remove("MovieActors");

            if (!ModelState.IsValid)
                return View(vm);

            Actor actor = new()
            {
                Name = vm.Name
            };

            if (vm.Img is not null && vm.Img.Length > 0)
            {
                var fileName = _actorService.SaveImg(vm.Img);
                if (fileName is not null)
                    actor.Img = fileName;
            }

            _context.Actors.Add(actor);
            _context.SaveChanges();

            TempData["success_notification"] = "Add Actor Successfully";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            var actor = _context.Actors.SingleOrDefault(e => e.Id == id);

            if (actor is null) return NotFound();

            return View(new UpdateActorVM()
            {
                Id = actor.Id,
                Name = actor.Name,
                CurrentImg = actor.Img
            });
        }

        [HttpPost]
        public IActionResult Update(UpdateActorVM vm)
        {
            ModelState.Remove("MovieActors");
            ModelState.Remove("CurrentImg");

            if (!ModelState.IsValid)
                return View(vm);

            var actor = _context.Actors.SingleOrDefault(e => e.Id == vm.Id);

            if (actor is null) return NotFound();

            actor.Name = vm.Name;

            if (vm.Img is not null && vm.Img.Length > 0)
            {
                _actorService.RemoveImg(actor.Img);
                var fileName = _actorService.SaveImg(vm.Img);
                if (fileName is not null) actor.Img = fileName;
            }

            _context.Actors.Update(actor);
            _context.SaveChanges();

            TempData["success_notification"] = "Update Actor Successfully";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var actor = _context.Actors.SingleOrDefault(e => e.Id == id);

            if (actor is null) return NotFound();

            _actorService.RemoveImg(actor.Img);

            _context.Actors.Remove(actor);
            _context.SaveChanges();

            TempData["success_notification"] = "Delete Actor Successfully";
            return RedirectToAction(nameof(Index));
        }
    }
}