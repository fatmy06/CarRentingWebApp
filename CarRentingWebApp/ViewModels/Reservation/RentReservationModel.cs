using System.ComponentModel.DataAnnotations;

namespace CarRentingWebApp.ViewModels.Reservation
{
    public class RentReservationModel
    {
        [Required]
        public int VehicleId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
    }
}
