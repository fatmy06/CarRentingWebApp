using CarRentingWebApp.ViewModels.Vehicle;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CarRentingWebApp.ViewModels.Reservation
{
    public class RentReservationCompositeViewModel
    {
        public VehicleViewModel? Vehicle { get; set; }

        public RentReservationModel ReservationInput { get; set; } = new RentReservationModel();
    }
}
