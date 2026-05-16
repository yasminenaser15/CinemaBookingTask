using System.ComponentModel.DataAnnotations;

namespace CinemaBookingTask.ViewModels
{
    public class ForgetPasswordVM
    {
        [Required]
        public string EmailOrUserName { get; set; }
    }
}