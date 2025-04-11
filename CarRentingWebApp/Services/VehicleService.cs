using CarRentingWebApp.Data.Models;
using CarRentingWebApp.Data;
using CarRentingWebApp.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using CarRentingWebApp.ViewModels.Vehicle;
using CarRentingWebApp.ViewModels.Reservation;

namespace CarRentingWebApp.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly ApplicationDbContext _context;

        public VehicleService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Vehicle>> GetAllVehiclesAsync()
        {
            return await _context.Vehicles.ToListAsync();
        }

        public async Task<IEnumerable<VehicleViewModel>> GetAllVehicleViewModelsAsync()
        {
            var vehicles = await _context.Vehicles.ToListAsync();

            var viewModels = vehicles.Select(v => new VehicleViewModel
            {
                Id = v.Id,
                Brand = v.Brand,
                Model = v.Model,
                ImageUrl = v.ImageUrl,
                Year = v.Year,
                PassengerSeats = v.PassengerSeats,
                Description = v.Description!,
                PricePerDay = v.PricePerDay
            }).ToList();

            return viewModels;
        }

        public async Task<Vehicle> GetVehicleByIdAsync(int id)
        {
            return await _context.Vehicles.FindAsync(id);
        }

        public async Task CreateVehicleAsync(AddVehicleViewModel model)
        {
            var vehicle = new Vehicle
            {
                Brand = model.Brand,
                Model = model.Model,
                Year = model.Year,
                ImageUrl = model.ImageUrl,
                PassengerSeats = model.PassengerSeats,
                Description = model.Description,
                PricePerDay = model.PricePerDay
            };

            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateVehicleAsync(Vehicle vehicle)
        {
            _context.Vehicles.Update(vehicle);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteVehicleAsync(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle != null)
            {
                _context.Vehicles.Remove(vehicle);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<EditVehicleViewModel> GetVehicleForEditAsync(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null)
            {
                return null;
            }

            return new EditVehicleViewModel
            {
                Id = vehicle.Id,
                Brand = vehicle.Brand,
                Model = vehicle.Model,
                ImageUrl = vehicle.ImageUrl,
                Year = vehicle.Year,
                PassengerSeats = vehicle.PassengerSeats,
                Description = vehicle.Description,
                PricePerDay = vehicle.PricePerDay
            };
        }

        public async Task UpdateVehicleAsync(EditVehicleViewModel model)
        {
            var vehicle = await _context.Vehicles.FindAsync(model.Id);

            vehicle!.Brand = model.Brand;
            vehicle.Model = model.Model;
            vehicle.Year = model.Year;
            vehicle.ImageUrl = model.ImageUrl;
            vehicle.PassengerSeats = model.PassengerSeats;
            vehicle.Description = model.Description;
            vehicle.PricePerDay = model.PricePerDay;

            _context.Vehicles.Update(vehicle);
            await _context.SaveChangesAsync();
        }

        public async Task<VehicleViewModel> GetVehicleViewModelByIdAsync(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null)
            {
                return null;
            }
            return new VehicleViewModel
            {
                Id = vehicle.Id,
                Brand = vehicle.Brand,
                Model = vehicle.Model,
                ImageUrl = vehicle.ImageUrl,
                Year = vehicle.Year,
                PassengerSeats = vehicle.PassengerSeats,
                Description = vehicle.Description,
                PricePerDay = vehicle.PricePerDay
            };
        }

        public async Task<IEnumerable<VehicleViewModel>> GetAllVehicleViewModelsFilteredAsync(DateTime? startDate, DateTime? endDate)
        {
            if (!startDate.HasValue || !endDate.HasValue)
            {
                return await GetAllVehicleViewModelsAsync();
            }

            var allVehicles = await _context.Vehicles.ToListAsync();

            var availableVehicles = allVehicles.Where(vehicle =>
            {
                var reservations = _context.Reservations.Where(r => r.VehicleId == vehicle.Id);
                bool isAvailable = !reservations.Any(r => startDate.Value < r.EndDate && endDate.Value > r.StartDate);
                return isAvailable;
            });

            var availableVehicleViewModels = availableVehicles.Select(v => new VehicleViewModel
            {
                Id = v.Id,
                Brand = v.Brand,
                Model = v.Model,
                ImageUrl = v.ImageUrl,
                Year = v.Year,
                PassengerSeats = v.PassengerSeats,
                Description = v.Description,
                PricePerDay = v.PricePerDay
            });

            return availableVehicleViewModels;
        }
    }
}
