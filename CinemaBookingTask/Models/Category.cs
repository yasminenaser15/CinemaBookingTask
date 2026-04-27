using System.ComponentModel.DataAnnotations;

namespace CinemaBookingTask.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<Movie> Movies { get; set; }
    }
}
