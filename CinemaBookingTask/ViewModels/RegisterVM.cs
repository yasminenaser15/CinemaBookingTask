using System.ComponentModel.DataAnnotations;

namespace CinemaBookingTask.ViewModels
{
    public class RegisterVM
    {
        [Required]
        public string FName { get; set; }

        [Required]
        public string LName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare(nameof(Password))]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public string? Address { get; set; }
    }
}
