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


        public bool IsAuthenticated => _contextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
        public string? UserName => _contextAccessor.HttpContext?.User?.Identity?.Name;
        public string? FirstName => _contextAccessor.HttpContext?.User?.FindFirst(nameof(FirstName))?.Value;
        public string? LastName => _contextAccessor.HttpContext?.User?.FindFirst(nameof(LastName))?.Value;
    }
}