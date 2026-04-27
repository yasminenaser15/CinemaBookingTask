using CinemaBookingTask.Models;

namespace CinemaBookingTask.ViewModels
{
    public class MovieWithRelatedVM
    {
        public IEnumerable<Movie> Movies { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public string? Query { get; set; }
        public IEnumerable<Category> Categories { get; set; } = new List<Category>();
        public IEnumerable<Cinema> Cinemas { get; set; } = new List<Cinema>();
    }
}
