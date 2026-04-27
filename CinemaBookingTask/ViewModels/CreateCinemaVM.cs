using System.ComponentModel.DataAnnotations;

namespace CinemaBookingTask.ViewModels
{
    public class CreateCinemaVM
    {
        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }

        public IFormFile? Img { get; set; }
    }
}
