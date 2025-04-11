namespace CarRentingWebApp.ViewModels.Reservation
{
    public class ReservationUserViewModel
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public string VehicleBrand { get; set; } = null!;
        public string VehicleModel { get; set; } = null!;
        public string? VehicleImageUrl { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = null!;
    }
}
