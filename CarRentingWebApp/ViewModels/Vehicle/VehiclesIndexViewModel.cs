namespace CarRentingWebApp.ViewModels.Vehicle
{
    public class VehiclesIndexViewModel
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public IEnumerable<VehicleViewModel> Vehicles { get; set; }
    }
}
