using System.ComponentModel.DataAnnotations;

namespace CinemaBookingTask.Models
{
    public class Cinema
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 100 characters")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }

        public string? Img { get; set; }  

        public ICollection<Movie> Movies { get; set; }
    }
}
