using CinemaBookingTask.DataAcess;
using CinemaBookingTask.Models;
using CinemaBookingTask.Services;
using CinemaBookingTask.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaBookingTask.Controllers
{
    public class MovieController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly MovieService _movieService;

        public MovieController(ApplicationDbContext context)
        {
            _context = context;
            _movieService = new();
        }

        public IActionResult Index(int page = 1, string? query = null,
            int? categoryId = null, int? cinemaId = null)
        {
            var movies = _context.Movies
                            .Include(e => e.Category)
                            .Include(e => e.Cinema)
                            .Include(e => e.MovieSubImgs)
                            .AsQueryable();

            if (query is not null)
                movies = movies.Where(e => e.Name.ToLower().Contains(query.ToLower().Trim()));

            if (categoryId is not null)
                movies = movies.Where(e => e.CategoryId == categoryId);

            if (cinemaId is not null)
                movies = movies.Where(e => e.CinemaId == cinemaId);

            int totalPages = (int)Math.Ceiling(movies.Count() / 5.0);
            movies = movies.Skip((page - 1) * 5).Take(5);

            return View(new MovieWithRelatedVM()
            {
                Movies = movies.AsEnumerable(),
                TotalPages = totalPages,
                CurrentPage = page,
                Query = query,
                Categories = _context.Categories.AsEnumerable(),
                Cinemas = _context.Cinemas.AsEnumerable()
            });
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new UpsertMovieVM()
            {
                Categories = _context.Categories.AsEnumerable(),
                Cinemas = _context.Cinemas.AsEnumerable(),
                Actors = _context.Actors.AsEnumerable()
            });
        }

        [HttpPost]
        public IActionResult Create(Movie movie, IFormFile? mainImg,
            List<IFormFile>? subImgs, List<int>? actorIds)
        {
            ModelState.Remove("MainImg");
            ModelState.Remove("MovieSubImgs");
            ModelState.Remove("MovieActors");
            ModelState.Remove("Category");
            ModelState.Remove("Cinema");

            if (!ModelState.IsValid)
                return View(new UpsertMovieVM()
                {
                    Movie = movie,
                    Categories = _context.Categories.AsEnumerable(),
                    Cinemas = _context.Cinemas.AsEnumerable(),
                    Actors = _context.Actors.AsEnumerable()
                });

            if (mainImg is not null && mainImg.Length > 0)
            {
                var fileName = _movieService.SaveImg(mainImg);
                if (fileName is not null)
                    movie.MainImg = fileName;
            }

            _context.Movies.Add(movie);
            _context.SaveChanges();

            // Save Actors
            if (actorIds is not null && actorIds.Any())
            {
                foreach (var actorId in actorIds)
                {
                    _context.MovieActors.Add(new MovieActor()
                    {
                        MovieId = movie.Id,
                        ActorId = actorId
                    });
                }
                _context.SaveChanges();
            }

            // Save SubImgs
            if (subImgs is not null && subImgs.Any())
            {
                foreach (var item in subImgs)
                {
                    if (item is not null && item.Length > 0)
                    {
                        var fileName = _movieService.SaveImg(item, MovieImgType.SubImg);
                        if (fileName is not null)
                        {
                            _context.MovieSubImgs.Add(new MovieSubImg()
                            {
                                SubImg = fileName,
                                MovieId = movie.Id
                            });
                        }
                    }
                }
                _context.SaveChanges();
            }

            TempData["success_notification"] = "Add Movie Successfully";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            var movie = _context.Movies.Include(e => e.MovieActors).SingleOrDefault(e => e.Id == id);
                            
            if (movie is null) return NotFound();

            return View(new UpsertMovieVM()
            {
                Movie = movie,
                MovieSubImgs = _context.MovieSubImgs.Where(e => e.MovieId == id).AsEnumerable(),
                Categories = _context.Categories.AsEnumerable(),
                Cinemas = _context.Cinemas.AsEnumerable(),
                Actors = _context.Actors.AsEnumerable()
            });
        }

        [HttpPost]
        public IActionResult Update(Movie movie, IFormFile? mainImg,
            List<IFormFile>? subImgs, List<int>? actorIds)
        {
            ModelState.Remove("MainImg");
            ModelState.Remove("MovieSubImgs");
            ModelState.Remove("MovieActors");
            ModelState.Remove("Category");
            ModelState.Remove("Cinema");

            if (!ModelState.IsValid)
                return View(new UpsertMovieVM()
                {
                    Movie = movie,
                    Categories = _context.Categories.AsEnumerable(),
                    Cinemas = _context.Cinemas.AsEnumerable(),
                    Actors = _context.Actors.AsEnumerable()
                });

            var movieInDb = _context.Movies
                                .Include(e => e.MovieActors)
                                .AsNoTracking()
                                .SingleOrDefault(e => e.Id == movie.Id);

            if (movieInDb is null) return NotFound();

            // Handle MainImg
            if (mainImg is not null && mainImg.Length > 0)
            {
                _movieService.RemoveImg(movieInDb.MainImg);
                var fileName = _movieService.SaveImg(mainImg);
                if (fileName is not null) movie.MainImg = fileName;
            }
            else
                movie.MainImg = movieInDb.MainImg;

            _context.Movies.Update(movie);
            _context.SaveChanges();

            // Update Actors
            if (actorIds is not null)
            {
                var oldActors = _context.MovieActors.Where(e => e.MovieId == movie.Id);
                _context.MovieActors.RemoveRange(oldActors);

                foreach (var actorId in actorIds)
                {
                    _context.MovieActors.Add(new MovieActor()
                    {
                        MovieId = movie.Id,
                        ActorId = actorId
                    });
                }
                _context.SaveChanges();
            }

            // Update SubImgs
            if (subImgs is not null && subImgs.Any())
            {
                var oldSubImgs = _context.MovieSubImgs.Where(e => e.MovieId == movie.Id);
                foreach (var item in oldSubImgs)
                    _movieService.RemoveImg(item.SubImg, MovieImgType.SubImg);

                _context.MovieSubImgs.RemoveRange(oldSubImgs);

                foreach (var item in subImgs)
                {
                    if (item is not null && item.Length > 0)
                    {
                        var fileName = _movieService.SaveImg(item, MovieImgType.SubImg);
                        if (fileName is not null)
                        {
                            _context.MovieSubImgs.Add(new MovieSubImg()
                            {
                                SubImg = fileName,
                                MovieId = movie.Id
                            });
                        }
                    }
                }
                _context.SaveChanges();
            }

            TempData["success_notification"] = "Update Movie Successfully";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var movie = _context.Movies.SingleOrDefault(e => e.Id == id);

            if (movie is null) return NotFound();

            // Remove SubImgs
            var subImgs = _context.MovieSubImgs.Where(e => e.MovieId == id);
            foreach (var item in subImgs)
                _movieService.RemoveImg(item.SubImg, MovieImgType.SubImg);
            _context.MovieSubImgs.RemoveRange(subImgs);

            // Remove Actors
            var movieActors = _context.MovieActors.Where(e => e.MovieId == id);
            _context.MovieActors.RemoveRange(movieActors);

            // Remove MainImg
            _movieService.RemoveImg(movie.MainImg);

            _context.Movies.Remove(movie);
            _context.SaveChanges();

            TempData["success_notification"] = "Delete Movie Successfully";
            return RedirectToAction(nameof(Index));
        }
    }
}
