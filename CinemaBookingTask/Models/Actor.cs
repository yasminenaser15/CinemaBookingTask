using System.ComponentModel.DataAnnotations;

namespace CinemaBookingTask.Models
{
    public class Actor
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string? Img { get; set; }

        public ICollection<MovieActor> MovieActors { get; set; }
    }
}
