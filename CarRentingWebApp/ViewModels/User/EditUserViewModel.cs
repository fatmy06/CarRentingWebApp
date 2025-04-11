using System.ComponentModel.DataAnnotations;

namespace CarRentingWebApp.ViewModels.User
{
    public class EditUserViewModel
    {
        public string Id { get; set; } = null!;

        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string FirstName { get; set; } = null!;

        [Required]
        public string LastName { get; set; } = null!;

        [Required]
        public string EGN { get; set; } = null!;
    }
}
