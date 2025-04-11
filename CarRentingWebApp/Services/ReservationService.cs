using CarRentingWebApp.Data;
using CarRentingWebApp.Data.Enums;
using CarRentingWebApp.Data.Models;
using CarRentingWebApp.Services.Contracts;
using CarRentingWebApp.ViewModels.Reservation;
using CarRentingWebApp.ViewModels.Vehicle;
using Microsoft.EntityFrameworkCore;

namespace CarRentingWebApp.Services
{
    public class ReservationService : IReservationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IVehicleService _vehicleService;

        public ReservationService(ApplicationDbContext context, IVehicleService vehicleService)
        {
            _context = context;
            _vehicleService = vehicleService;
        }

        public async Task AddReservation(Reservation reservation)
        {
            reservation.Status = ReservationStatus.Pending;
            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();
        }

        public async Task<CreateReservationViewModel?> GetReservationViewModelAsync(int vehicleId)
        {
            var vehicle = await _vehicleService.GetVehicleByIdAsync(vehicleId);
            if (vehicle == null)
            {
                return null;
            }

            return new CreateReservationViewModel
            {
                VehicleId = vehicle.Id,
                Brand = vehicle.Brand,
                Model = vehicle.Model,
                ImageUrl = vehicle.ImageUrl,
                PricePerDay = vehicle.PricePerDay,
                Year = vehicle.Year,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(1)
            };
        }

        public async Task<bool> IsVehicleAvailable(int vehicleId, DateTime startDate, DateTime endDate)
        {
            var reservations = await _context.Reservations
                .Where(r => r.VehicleId == vehicleId)
                .ToListAsync();

            foreach (var reservation in reservations)
            {
                if (startDate < reservation.EndDate && endDate > reservation.StartDate)
                {
                    return false;
                }
            }
            return true;
        }

        public async Task<RentReservationCompositeViewModel?> GetCompositeReservationModelAsync(int vehicleId)
        {
            var vehicle = await _context.Vehicles.FirstOrDefaultAsync(v => v.Id == vehicleId);
            if (vehicle == null)
            {
                return null;
            }

            var compositeModel = new RentReservationCompositeViewModel
            {
                Vehicle = new VehicleViewModel
                {
                    Id = vehicle.Id,
                    Brand = vehicle.Brand,
                    Model = vehicle.Model,
                    ImageUrl = vehicle.ImageUrl,
                    Year = vehicle.Year,
                    PricePerDay = vehicle.PricePerDay,
                    Description = vehicle.Description
                },
                ReservationInput = new RentReservationModel
                {
                    VehicleId = vehicle.Id,
                    StartDate = DateTime.Today,
                    EndDate = DateTime.Today.AddDays(1)
                }
            };
            return compositeModel;
        }

        public async Task<IEnumerable<ReservationAdminViewModel>> GetAllReservationsForAdminAsync()
        {
            return await _context.Reservations
                .Include(r => r.Vehicle)
                .Include(r => r.User)
                .Select(r => new ReservationAdminViewModel
                {
                    Id = r.Id,
                    VehicleBrand = r.Vehicle.Brand,
                    VehicleModel = r.Vehicle.Model,
                    VehicleImageUrl = r.Vehicle.ImageUrl,
                    StartDate = r.StartDate,
                    EndDate = r.EndDate,
                    ReservationStatus = r.Status, 
                    UserFullName = r.User.FirstName + " " + r.User.LastName,
                    UserEmail = r.User.Email
                })
                .ToListAsync();
        }

        public async Task<bool> CreatePendingReservationAsync(RentReservationModel input, string userId)
        {
            var reservation = new Reservation
            {
                VehicleId = input.VehicleId,
                UserId = userId,
                StartDate = input.StartDate,
                EndDate = input.EndDate,
                Status = ReservationStatus.Pending
            };

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateReservationStatusAsync(int reservationId, ReservationStatus newStatus)
        {
            var reservation = await _context.Reservations.FindAsync(reservationId);
            if (reservation == null)
            {
                return false;
            }

            reservation.Status = newStatus;

            _context.Reservations.Update(reservation);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<ReservationUserViewModel>> GetReservationsByUserIdAsync(string userId)
        {
            return await _context.Reservations
                .Include(r => r.Vehicle)
                .Where(r => r.UserId == userId)
                .Select(r => new ReservationUserViewModel
                {
                    Id = r.Id,
                    VehicleId = r.VehicleId,
                    VehicleBrand = r.Vehicle.Brand,
                    VehicleModel = r.Vehicle.Model,
                    VehicleImageUrl = r.Vehicle.ImageUrl,
                    StartDate = r.StartDate,
                    EndDate = r.EndDate,
                    Status = r.Status.ToString()
                })
                .ToListAsync();
        }

        public async Task<bool> RemoveReservationAsync(int reservationId, string userId)
        {
            var reservation = await _context.Reservations.FirstOrDefaultAsync(r => r.Id == reservationId && r.UserId == userId);
            if (reservation == null)
            {
                return false;
            }

            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveReservationAsAdminAsync(int reservationId)
        {
            var reservation = await _context.Reservations.FirstOrDefaultAsync(r => r.Id == reservationId);
            if (reservation == null)
            {
                return false;
            }

            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
