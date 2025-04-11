using CarRentingWebApp.Data.Models;
using CarRentingWebApp.ViewModels.User;
using Microsoft.AspNetCore.Identity;

namespace CarRentingWebApp.Services.Contracts
{
    public interface IUserService
    {
        public Task<ApplicationUser> CreateUserAsync(CreateUserViewModel model);

        public Task Logout();

        public Task<SignInResult> Login(LoginViewModel model);

        Task<bool> IsEGNUniqueAsync(string egn);
        Task<IEnumerable<UserAdminViewModel>> GetAllUsersAsync();
        Task<UserAdminViewModel?> GetUserByIdAsync(string userId);
        Task<IdentityResult> UpdateUserAsync(EditUserViewModel model);
        Task<IdentityResult> DeleteUserAsync(string userId);

        Task<ApplicationUser> CreateUserWithoutSignInAsync(CreateUserViewModel model);
    }
}
