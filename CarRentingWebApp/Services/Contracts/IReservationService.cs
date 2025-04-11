using CarRentingWebApp.Data.Enums;
using CarRentingWebApp.Data.Models;
using CarRentingWebApp.ViewModels.Reservation;

namespace CarRentingWebApp.Services.Contracts
{
    public interface IReservationService
    {
        Task<CreateReservationViewModel?> GetReservationViewModelAsync(int vehicleId);
        Task<bool> IsVehicleAvailable(int vehicleId, DateTime startDate, DateTime endDate);
        Task AddReservation(Reservation reservation);
        Task<RentReservationCompositeViewModel?> GetCompositeReservationModelAsync(int vehicleId);
        Task<IEnumerable<ReservationAdminViewModel>> GetAllReservationsForAdminAsync();
        Task<bool> CreatePendingReservationAsync(RentReservationModel input, string userId);
        Task<bool> UpdateReservationStatusAsync(int reservationId, ReservationStatus newStatus);
        Task<IEnumerable<ReservationUserViewModel>> GetReservationsByUserIdAsync(string userId);
        Task<bool> RemoveReservationAsync(int reservationId, string userId);
        Task<bool> RemoveReservationAsAdminAsync(int reservationId);
    }
}
