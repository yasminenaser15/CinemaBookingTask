using CinemaBookingTask.Models;

namespace CinemaBookingTask.ViewModels
{
    public class ActorWithRelatedVM
    {
        public IEnumerable<Actor> Actors { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public string? Query { get; set; }
    }
}
