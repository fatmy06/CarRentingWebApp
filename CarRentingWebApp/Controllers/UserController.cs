using CarRentingWebApp.Data.Models;
using CarRentingWebApp.Services;
using CarRentingWebApp.Services.Contracts;
using CarRentingWebApp.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CarRentingWebApp.Controllers
{
    public class UserController : Controller
    {
         private readonly IUserService userService;
         private readonly IReservationService reservationService;
         private readonly SignInManager<ApplicationUser> signInManager;

        public UserController(IUserService userService, SignInManager<ApplicationUser> signInManager, IReservationService reservationService)
        {
             this.userService = userService;
             this.signInManager = signInManager;
             this.reservationService = reservationService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool isUnique = await userService.IsEGNUniqueAsync(model.EGN);
                if (!isUnique)
                {
                    ModelState.AddModelError(nameof(model.EGN), "A user with this EGN already exists.");
                    return View(model);
                }

                var user = await userService.CreateUserAsync(model);
                if (user != null)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Registration failed. Please try again.");
            }

            return View(model);
        }

        [HttpGet]

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            string returnUrl = Url.Content("~/");


            if (ModelState.IsValid)
            {
                var result = await userService.Login(model);
                if (result.Succeeded)
                {
                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = true });
                }
                if (result.IsLockedOut)
                {
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            return RedirectToAction(nameof(Index), "Home");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminIndex()
        {
            var users = await userService.GetAllUsersAsync();
            return View(users);
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var model = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                EGN = user.EGN
            };
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await userService.UpdateUserAsync(model);
                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "User updated successfully.";
                    return RedirectToAction("AdminIndex");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var result = await userService.DeleteUserAsync(id);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "User deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Error deleting user.";
            }
            return RedirectToAction("AdminIndex");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AdminCreateUser()
        {
            return View(new CreateUserViewModel());
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminCreateUser(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userService.CreateUserWithoutSignInAsync(model);
                if (user != null)
                {
                    TempData["SuccessMessage"] = "User created successfully.";
                    return RedirectToAction("AdminIndex", "User");
                }
                ModelState.AddModelError(string.Empty, "User creation failed. Please try again.");
            }

            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> MyRents()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var reservations = await reservationService.GetReservationsByUserIdAsync(userId);


            return View(reservations);
        }
    }
}
