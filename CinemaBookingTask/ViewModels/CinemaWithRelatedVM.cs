using CinemaBookingTask.Models;

namespace CinemaBookingTask.ViewModels
{
    public class CinemaWithRelatedVM
    {
        public IEnumerable<Cinema> Cinemas { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public string? Query { get; set; }
    }
}
