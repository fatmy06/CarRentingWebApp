using CarRentingWebApp.Data.Enums;

namespace CarRentingWebApp.ViewModels.Reservation
{
    public class ReservationAdminViewModel
    {
        public int Id { get; set; }
        public string VehicleBrand { get; set; } = null!;
        public string VehicleModel { get; set; } = null!;
        public string? VehicleImageUrl { get; set; }
        public string UserFullName { get; set; } = null!;
        public string UserEmail { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ReservationStatus ReservationStatus { get; set; }
    }
}
