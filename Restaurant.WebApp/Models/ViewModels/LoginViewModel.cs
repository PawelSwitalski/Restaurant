using System.ComponentModel.DataAnnotations;

namespace Restaurant.WebApp.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }

    }
}
