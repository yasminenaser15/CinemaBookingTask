using System.ComponentModel.DataAnnotations;

namespace CinemaBookingTask.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 50 characters")]
        public string Name { get; set; }

        public ICollection<Movie> Movies { get; set; }
    }
}
