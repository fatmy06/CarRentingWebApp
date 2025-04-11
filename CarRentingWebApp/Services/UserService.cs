using CarRentingWebApp.Data;
using CarRentingWebApp.Data.Models;
using CarRentingWebApp.Services.Contracts;
using CarRentingWebApp.ViewModels.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CarRentingWebApp.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public UserService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
        }

        public async Task<ApplicationUser> CreateUserAsync(CreateUserViewModel model)
        {
            ApplicationUser user = new()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                EGN = model.EGN,
                Email = model.Email,
                UserName = model.Email
            };

            var result = await userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                if (userManager.Users.Count() <= 1)
                {
                    IdentityRole roleUser = new IdentityRole() { Name = GlobalConstants.UserRole };
                    IdentityRole roleAdmin = new IdentityRole() { Name = GlobalConstants.AdminRole };
                    await roleManager.CreateAsync(roleUser);
                    await roleManager.CreateAsync(roleAdmin);
                    await userManager.AddToRoleAsync(user, GlobalConstants.AdminRole);
                }
                else
                {
                    await userManager.AddToRoleAsync(user, GlobalConstants.UserRole);
                }

                return user;
            }

            return null;
        }

        public async Task<SignInResult> Login(LoginViewModel model)
        {
            return await signInManager.PasswordSignInAsync(model.Email, model.Password, true, lockoutOnFailure: false);
        }

        public async Task Logout()
        {
            await signInManager.SignOutAsync();
        }

        public async Task<bool> IsEGNUniqueAsync(string egn)
        {
            var user = await userManager.Users.FirstOrDefaultAsync(u => u.EGN == egn);
            return user == null;
        }

        public async Task<IEnumerable<UserAdminViewModel>> GetAllUsersAsync()
        {
            return await userManager.Users
                .Select(u => new UserAdminViewModel
                {
                    Id = u.Id,
                    Email = u.Email,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    EGN = u.EGN
                })
                .ToListAsync();
        }

        public async Task<UserAdminViewModel?> GetUserByIdAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return null;
            return new UserAdminViewModel
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                EGN = user.EGN
            };
        }

        public async Task<IdentityResult> UpdateUserAsync(EditUserViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });
            }
            user.Email = model.Email;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.EGN = model.EGN;

            return await userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> DeleteUserAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });
            }
            return await userManager.DeleteAsync(user);
        }

        public async Task<ApplicationUser> CreateUserWithoutSignInAsync(CreateUserViewModel model)
        {
            ApplicationUser user = new()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                EGN = model.EGN,
                Email = model.Email,
                UserName = model.Email
            };

            var result = await userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                if (userManager.Users.Count() <= 1)
                {
                    IdentityRole roleUser = new IdentityRole() { Name = GlobalConstants.UserRole };
                    IdentityRole roleAdmin = new IdentityRole() { Name = GlobalConstants.AdminRole };
                    await roleManager.CreateAsync(roleUser);
                    await roleManager.CreateAsync(roleAdmin);
                    await userManager.AddToRoleAsync(user, GlobalConstants.AdminRole);
                }
                else
                {
                    await userManager.AddToRoleAsync(user, GlobalConstants.UserRole);
                }

                return user;
            }

            return null;
        }
    }
}
