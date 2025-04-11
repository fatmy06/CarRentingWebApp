namespace CarRentingWebApp.ViewModels.Vehicle
{
    public class VehicleViewModel
    {
        public int Id { get; set; }
        public string Brand { get; set; } = null!;
        public string Model { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public int Year { get; set; }
        public int PassengerSeats { get; set; }
        public string? Description { get; set; }
        public decimal PricePerDay { get; set; }

    }
}
