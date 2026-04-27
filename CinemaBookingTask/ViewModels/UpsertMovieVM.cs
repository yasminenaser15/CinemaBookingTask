using CinemaBookingTask.Models;

namespace CinemaBookingTask.ViewModels
{
    public class UpsertMovieVM
    {
        public Movie Movie { get; set; } = new();
        public IEnumerable<Category> Categories { get; set; } = new List<Category>();
        public IEnumerable<Cinema> Cinemas { get; set; } = new List<Cinema>();
        public IEnumerable<Actor> Actors { get; set; } = new List<Actor>();
        public IEnumerable<MovieSubImg> MovieSubImgs { get; set; } = new List<MovieSubImg>();
    }
}
