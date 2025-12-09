using Microsoft.AspNetCore.Identity;

namespace Hoshiko.Infrastructure.Identity
{
    public class AppUser : IdentityUser
    {
        public string FirstName {get;set;} = null!;
        public string LastName {get;set;} = null!;
    }
}