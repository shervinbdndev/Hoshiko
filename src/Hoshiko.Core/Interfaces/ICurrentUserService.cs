namespace Hoshiko.Core.Interfaces
{
    public interface ICurrentUserService
    {
        bool IsAuthenticated {get;}
        string? UserName {get;}
        string? FirstName {get;}
        string? LastName {get;}
        string Role {get;}
        bool IsAdmin {get;}
    }
}