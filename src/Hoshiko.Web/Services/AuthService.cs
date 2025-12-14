using System.Security.Claims;
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

            await EnsureUserClaimsAsync(user);

            await _signInManager.SignInAsync(user, isPersistent: false);

            return true;
        }


        public async Task<bool> LoginAsync(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return false;

            var result = await _signInManager.PasswordSignInAsync(user.UserName!, password, false, false);
            if (!result.Succeeded) return false;

            await EnsureUserClaimsAsync(user);

            await _signInManager.SignInAsync(user, isPersistent: false);

            return true;
        }


        public async Task LogoutAsync() => await _signInManager.SignOutAsync();


        private async Task EnsureUserClaimsAsync(AppUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var claimsToAdd = new List<Claim>();

            if (!userClaims.Any(c => c.Type == nameof(AppUser.FirstName))) claimsToAdd.Add(new Claim(nameof(AppUser.FirstName), user.FirstName ?? ""));
            if (!userClaims.Any(c => c.Type == nameof(AppUser.LastName))) claimsToAdd.Add(new Claim(nameof(AppUser.LastName), user.LastName ?? ""));
            if (claimsToAdd.Any()) await _userManager.AddClaimsAsync(user, claimsToAdd);
        }
    }
}