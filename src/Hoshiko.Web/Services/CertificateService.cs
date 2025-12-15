using Hoshiko.Core.Interfaces;
using Hoshiko.Domain.Entities;
using Hoshiko.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hoshiko.Web.Services
{
    public class CertificateService : ICertificateService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUser;

        public CertificateService(ApplicationDbContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }


        public async Task<Certificate?> GetUserCertificateAsync(string userId) => await _context.Certificates.FirstOrDefaultAsync(x => x.UserId == userId && !x.IsRevoked);
        public async Task<int> GetUserQuizScoreAsync(string userId) => await _context.UserQuizAnswers.Include(a => a.Quiz).Where(a => a.UserId == userId && a.SelectedOption == a.Quiz.CorrectOption).CountAsync();
        public async Task<int> GetTotalQuizCountAsync() => await _context.Quizzes.CountAsync();
        public async Task<bool> HasAnyQuizAttemptAsync(string userId) => await _context.UserQuizAnswers.AnyAsync(x => x.UserId == userId);

        public async Task<Certificate?> TryIssueCertificateAsync(string userId)
        {
            var totalStages = await _context.Stages.CountAsync();
            var completedStages = await _context.UserStageProgresses
                .CountAsync(x =>
                    x.UserId == userId &&
                    x.IsLearnCompleted &&
                    x.IsQuizCompleted
            );

            if (completedStages < totalStages) return null;

            var totalQuizzes = await _context.Quizzes.CountAsync();
            var correctAnswers = await _context.UserQuizAnswers
                .Include(a => a.Quiz)
                .CountAsync(a =>
                    a.UserId == userId &&
                    a.SelectedOption == a.Quiz.CorrectOption
            );

            var scorePercent = totalQuizzes == 0 ? 0 : (correctAnswers * 100) / totalQuizzes;

            if (scorePercent < 70) return null;

            var existing = await _context.Certificates.FirstOrDefaultAsync(c => c.UserId == userId && !c.IsRevoked);
            if (existing != null) return existing;

            var fullName = $"{_currentUser.FirstName} {_currentUser.LastName}".Trim();
            if (string.IsNullOrEmpty(fullName)) fullName = _currentUser.UserName ?? "کاربر";

            var cert = new Certificate
            {
                UserId = userId,
                UserName = _currentUser.UserName ?? "کاربر",
                FullName = fullName,
                CertificateCode = $"HSK-{Guid.NewGuid().ToString("N")[..6]}".ToUpper(),
                IssuedAt = DateTime.UtcNow
            };

            _context.Certificates.Add(cert);
            await _context.SaveChangesAsync();

            return cert;
        }



        public async Task<bool> CanIssueCertificateAsync(string userId)
        {
            if (!await HasAnyQuizAttemptAsync(userId)) return false;

            var correctCount = await GetUserQuizScoreAsync(userId);
            var total = await GetTotalQuizCountAsync();

            if (total == 0) return false;

            var percent = (correctCount * 100) / total;

            return percent >= 70;
        }



        public async Task<Certificate?> GetByCertificateCodeAsync(string code)
        {
            if (string.IsNullOrWhiteSpace(code)) return null;

            return await _context.Certificates
                .AsNoTracking()
                .FirstOrDefaultAsync(c =>
                    EF.Functions.Like(c.CertificateCode, code) &&
                    !c.IsRevoked
                );
        }
    }
}