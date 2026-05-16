using System.ComponentModel.DataAnnotations;

namespace CinemaBookingTask.ViewModels
{
    public class LoginVM
    {
        [Required]
        public string EmailOrUserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}