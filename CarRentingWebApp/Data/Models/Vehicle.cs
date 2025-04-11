using System.ComponentModel.DataAnnotations;

namespace CarRentingWebApp.Data.Models
{
    public class Vehicle
    {
         [Required]
         [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Brand { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string Model { get; set; } = null!;

        [Required]
        public string ImageUrl { get; set; } = null!;

        [Required]
        public int Year { get; set; }

        [Required]
        public int PassengerSeats { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required]
        public decimal PricePerDay { get; set; }
    }
}
