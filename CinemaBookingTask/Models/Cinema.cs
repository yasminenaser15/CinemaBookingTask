using System.ComponentModel.DataAnnotations;

namespace CinemaBookingTask.Models
{
    public class Cinema
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }

        public string? Img { get; set; }  

        public ICollection<Movie> Movies { get; set; }
    }
}
