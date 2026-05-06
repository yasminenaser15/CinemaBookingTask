using CinemaBookingTask.Validations;
using System.ComponentModel.DataAnnotations;

namespace CinemaBookingTask.Models
{
    public class Movie
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 100 characters")]
        public string Name { get; set; }


        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }


        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, 10000, ErrorMessage = "Price must be between 0.01 and 10000")]
        public double Price { get; set; }

        public string? Status { get; set; }

      
        [Required(ErrorMessage = "DateTime is required")]
        [FutureDate(ErrorMessage = "DateTime must be in the future!")]
        public DateTime DateTime { get; set; }

        public string? MainImg { get; set; }

        public ICollection<MovieSubImg> MovieSubImgs { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        [Required(ErrorMessage = "Cinema is required")]
        public int CinemaId { get; set; }
        public Cinema Cinema { get; set; }
         
        public ICollection<MovieActor> MovieActors { get; set; }
    }
}
