using System.ComponentModel.DataAnnotations;

namespace CinemaBookingTask.ViewModels
{
    public class CreateActorVM
    {
        [Required]
        public string Name { get; set; }
        public IFormFile? Img { get; set; }
    }
}
