using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Hoshiko.Infrastructure.Identity;

namespace Hoshiko.Web.Services
{
    public class UserService
    {
        private readonly UserManager<AppUser> _userManager;

        public UserService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }


        public async Task<List<AppUser>> GetAllUsersAsync() => await _userManager.Users.ToListAsync();
    }
}