using System.ComponentModel.DataAnnotations;

namespace CinemaBookingTask.ViewModels
{
    public class UpdateCinemaVM
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }

        public IFormFile? Img { get; set; }
        public string? CurrentImg { get; set; }
    }
}
