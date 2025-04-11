using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace CarRentingWebApp.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
         [Required]
         [MaxLength(50)]
        public string FirstName { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } = null!;

         [Required]
         [MaxLength(10)]
        public string EGN { get; set; } = null!;

         public ICollection<IdentityUserRole<string>> Roles { get; set; } = new HashSet<IdentityUserRole<string>>();
    }
}
