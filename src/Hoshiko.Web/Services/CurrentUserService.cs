using System.Security.Claims;
using Hoshiko.Core.Interfaces;

namespace Hoshiko.Web.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public CurrentUserService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }


        private ClaimsPrincipal? CurrentPrincipal => _contextAccessor.HttpContext?.User;

        
        public bool IsAuthenticated => CurrentPrincipal?.Identity?.IsAuthenticated ?? false;
        public string? UserName => CurrentPrincipal?.Identity?.Name;
        public string? FirstName => CurrentPrincipal?.FindFirst(nameof(FirstName))?.Value;
        public string? LastName => CurrentPrincipal?.FindFirst(nameof(LastName))?.Value;
        public string Role => CurrentPrincipal?.FindFirst(ClaimTypes.Role)?.Value ?? "User";
        public bool IsAdmin => CurrentPrincipal?.IsInRole("Admin") ?? false;
    }
}