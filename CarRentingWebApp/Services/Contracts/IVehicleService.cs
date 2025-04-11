using CarRentingWebApp.Data.Models;
using CarRentingWebApp.ViewModels.Reservation;
using CarRentingWebApp.ViewModels.Vehicle;

namespace CarRentingWebApp.Services.Contracts
{
    public interface IVehicleService
    {
        Task<IEnumerable<Vehicle>> GetAllVehiclesAsync();
        Task<IEnumerable<VehicleViewModel>> GetAllVehicleViewModelsAsync();
        Task<VehicleViewModel> GetVehicleViewModelByIdAsync(int id);
        Task<Vehicle> GetVehicleByIdAsync(int id);
        Task CreateVehicleAsync(AddVehicleViewModel model);
        Task UpdateVehicleAsync(Vehicle vehicle);
        Task DeleteVehicleAsync(int id);
        Task<EditVehicleViewModel> GetVehicleForEditAsync(int id);
        Task UpdateVehicleAsync(EditVehicleViewModel model);
        Task<IEnumerable<VehicleViewModel>> GetAllVehicleViewModelsFilteredAsync(DateTime? startDate, DateTime? endDate);
    }
}
