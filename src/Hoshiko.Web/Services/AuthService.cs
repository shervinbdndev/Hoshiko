using Hoshiko.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Hoshiko.Infrastructure.Identity;

namespace Hoshiko.Web.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AuthService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


        public async Task<bool> RegisterAsync(string username, string password, string confirmPassword, string firstName, string lastName)
        {
            if (password != confirmPassword) return false;

            var user = new AppUser
            {
                UserName = username,
                FirstName = firstName,
                LastName = lastName
            };
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded) return false;

            await _signInManager.SignInAsync(user, false);
            return true;
        }


        public async Task<bool> LoginAsync(string username, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(username, password, false, false);
            return result.Succeeded;
        }


        public async Task LogoutAsync() => await _signInManager.SignOutAsync();
    }
}