using Hoshiko.Domain.Entities;

namespace Hoshiko.Core.Interfaces
{
    public interface ICertificateService
    {
        Task<Certificate?> TryIssueCertificateAsync(string userId);
        Task<Certificate?> GetUserCertificateAsync(string userId);
        Task<int> GetUserQuizScoreAsync(string userId);
        Task<int> GetTotalQuizCountAsync();
        Task<bool> HasAnyQuizAttemptAsync(string userId);
    }
}