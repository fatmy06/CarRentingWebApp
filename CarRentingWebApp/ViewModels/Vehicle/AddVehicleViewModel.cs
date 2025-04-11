using System.ComponentModel.DataAnnotations;

namespace CarRentingWebApp.ViewModels.Vehicle
{
    public class AddVehicleViewModel
    {
        [Required(ErrorMessage = "Brand is required.")]
        [StringLength(50, MinimumLength = 2)]
        public string Brand { get; set; } = null!;

        [Required(ErrorMessage = "Model is required.")]
        [StringLength(50)]
        public string Model { get; set; } = null!;

        [Required]
        [Display(Name = "Image URL")]
        [Url(ErrorMessage = "Please enter a valid URL.")]
        public string ImageUrl { get; set; } = null!;

        [Required(ErrorMessage = "Year is required.")]
        [Range(1900, 2100, ErrorMessage = "Year must be between 1900 and 2100.")]
        public int Year { get; set; }

        [Required(ErrorMessage = "Passenger seats are required.")]
        [Range(1, 100, ErrorMessage = "Passenger seats must be between 1 and 100.")]
        public int PassengerSeats { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Price per day is required.")]
        public decimal PricePerDay { get; set; }
    }
}
