using CarRentingWebApp.Services.Contracts;
using CarRentingWebApp.ViewModels.Vehicle;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRentingWebApp.Controllers
{
    public class VehicleController : Controller
    {
        private readonly IVehicleService vehicleService;
        public VehicleController(IVehicleService vehicleService)
        {
                this.vehicleService = vehicleService;
        }

        [Authorize]
        public async Task<IActionResult> All(DateTime? startDate, DateTime? endDate)
        {
            var vehicleViewModels = await vehicleService.GetAllVehicleViewModelsFilteredAsync(startDate, endDate);

            var viewModel = new VehiclesIndexViewModel
            {
                StartDate = startDate,
                EndDate = endDate,
                Vehicles = vehicleViewModels
            };

            return View(viewModel);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create(AddVehicleViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                await vehicleService.CreateVehicleAsync(viewModel);
                return RedirectToAction("All");
            }
            return View(viewModel);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await vehicleService.GetVehicleForEditAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(EditVehicleViewModel viewModel)
        {
            if (viewModel == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await vehicleService.UpdateVehicleAsync(viewModel);
                return RedirectToAction("All");
            }
            return View(viewModel);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var model = await vehicleService.GetVehicleViewModelByIdAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var model = await vehicleService.GetVehicleByIdAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            await vehicleService.DeleteVehicleAsync(id);
            return RedirectToAction("All");
        }

        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            var model = await vehicleService.GetVehicleViewModelByIdAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }
    }
}
