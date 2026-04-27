using System.ComponentModel.DataAnnotations;

namespace CinemaBookingTask.Models
{
    public class Movie
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }

        public double Price { get; set; }

        public string? Status { get; set; }

        public DateTime DateTime { get; set; }

        public string? MainImg { get; set; }

        public ICollection<MovieSubImg> MovieSubImgs { get; set; }

        // Foreign Keys
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public int CinemaId { get; set; }
        public Cinema Cinema { get; set; }
         
        public ICollection<MovieActor> MovieActors { get; set; }
    }
}
