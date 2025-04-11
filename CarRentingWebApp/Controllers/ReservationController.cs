using CarRentingWebApp.Data.Enums;
using CarRentingWebApp.Data.Models;
using CarRentingWebApp.Services;
using CarRentingWebApp.Services.Contracts;
using CarRentingWebApp.ViewModels.Reservation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CarRentingWebApp.Controllers
{
    public class ReservationController : Controller
    {
       private readonly IReservationService _reservationService;
       private readonly IVehicleService _vehicleService;

        public ReservationController(IReservationService reservationService, IVehicleService vehicleService)
        {
            _reservationService = reservationService;
            _vehicleService = vehicleService;
        }

       
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Rent(int vehicleId)
        {
            var compositeModel = await _reservationService.GetCompositeReservationModelAsync(vehicleId);
            if (compositeModel == null)
            {
                return NotFound();
            }
            return View(compositeModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Rent(RentReservationCompositeViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                var vehicleDetails = await _vehicleService.GetVehicleViewModelByIdAsync(viewModel.ReservationInput.VehicleId);
                if (vehicleDetails != null)
                {
                    viewModel.Vehicle = vehicleDetails;
                }
                return View(viewModel);
            }

            if (viewModel.ReservationInput.StartDate >= viewModel.ReservationInput.EndDate)
            {
                ModelState.AddModelError(string.Empty, "The start date must be before the end date.");
                var vehicleDetails = await _vehicleService.GetVehicleViewModelByIdAsync(viewModel.ReservationInput.VehicleId);
                if (vehicleDetails != null)
                {
                    viewModel.Vehicle = vehicleDetails;
                }
                return View(viewModel);
            }

            bool isAvailable = await _reservationService.IsVehicleAvailable(
                viewModel.ReservationInput.VehicleId,
                viewModel.ReservationInput.StartDate,
                viewModel.ReservationInput.EndDate);
            if (!isAvailable)
            {
                ModelState.AddModelError(string.Empty, "The selected dates conflict with an existing reservation.");
                var vehicleDetails = await _vehicleService.GetVehicleViewModelByIdAsync(viewModel.ReservationInput.VehicleId);
                if (vehicleDetails != null)
                {
                    viewModel.Vehicle = vehicleDetails;
                }
                return View(viewModel);
            }

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            bool creationSucceeded = await _reservationService.CreatePendingReservationAsync(viewModel.ReservationInput, currentUserId);

            if (creationSucceeded)
            {
                TempData["SuccessMessage"] = "Your rental request has been submitted and is pending approval.";
                return RedirectToAction("MyRents", "User");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Failed to process your rental request. Please try again.");
                return View(viewModel);
            }

        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminIndex()
        {
            var reservations = await _reservationService.GetAllReservationsForAdminAsync();
            return View(reservations);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Approve(int reservationId)
        {
            bool updateSucceeded = await _reservationService.UpdateReservationStatusAsync(reservationId, ReservationStatus.Approved);

            if (updateSucceeded)
            {
                TempData["SuccessMessage"] = "The reservation has been approved.";
            }
            else
            {
                TempData["ErrorMessage"] = "Error: Unable to approve the reservation.";
            }

            return RedirectToAction("AdminIndex");
        }

        [Authorize]
        public async Task<IActionResult> RemoveAsUser(int reservationId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            bool removalSucceeded = await _reservationService.RemoveReservationAsync(reservationId, userId);

            if (removalSucceeded)
            {
                TempData["SuccessMessage"] = "Reservation removed successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Unable to remove the reservation.";
            }

            return RedirectToAction("MyRents", "User");
        }

        [Authorize]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveAsAdmin(int reservationId)
        {
            bool removalSucceeded = await _reservationService.RemoveReservationAsAdminAsync(reservationId);

            if (removalSucceeded)
            {
                TempData["SuccessMessage"] = "Reservation removed successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Unable to remove the reservation.";
            }

            return RedirectToAction("AdminIndex", "Reservation");
        }
    }
}
