namespace Hoshiko.Core.Interfaces
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(string username, string password, string confirmPassword, string firstName, string lastName);
        Task<bool> LoginAsync(string username, string password);
        Task LogoutAsync();
    }
}