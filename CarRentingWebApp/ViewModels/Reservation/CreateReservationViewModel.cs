using System.ComponentModel.DataAnnotations;

namespace CarRentingWebApp.ViewModels.Reservation
{
    public class CreateReservationViewModel
    {
        public int VehicleId { get; set; }
        public string Brand { get; set; } = null!;
         public string Model { get; set; } = null!;
         public string? Description { get; set; }
         public string ImageUrl { get; set; } = null!;
         public decimal PricePerDay { get; set; }
         public int Year { get; set; }

        
        
         [Required(ErrorMessage = "Start date is required")]
         [DataType(DataType.Date)]
         public DateTime StartDate { get; set; }

         [Required(ErrorMessage = "End date is required")]
         [DataType(DataType.Date)]
          public DateTime EndDate { get; set; }
    }
}
