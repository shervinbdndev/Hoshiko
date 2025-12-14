using Hoshiko.Core.Interfaces;
using Hoshiko.Domain.Entities;
using Hoshiko.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hoshiko.Web.Services
{
    public class StageProgressService : IStageProgressService
    {
        private readonly ApplicationDbContext _context;

        public StageProgressService(ApplicationDbContext context)
        {
            _context = context;
        }



        public async Task<Stage?> GetNextStageForUserAsync(string userId)
        {
            var completedStageIds = await _context.UserStageProgresses
                .Where(p => p.UserId == userId && p.IsQuizCompleted && p.IsLearnCompleted)
                .Select(p => p.StageId)
                .ToListAsync();

            return await _context.Stages
                .Where(s => !completedStageIds.Contains(s.Id))
                .OrderBy(s => s.Id)
                .Include(s => s.Learns)
                .Include(s => s.Quizzes)
                .FirstOrDefaultAsync();
        }



        public async Task<bool> CanAccessStageAsync(string userId, int stageId)
        {
            var stages = await _context.Stages.OrderBy(s => s.Id).ToListAsync();
            var targetIndex = stages.FindIndex(s => s.Id == stageId);
            if (targetIndex < 0) return false;

            if (targetIndex == 0) return true;

            var previousStage = stages[targetIndex - 1];
            var progress = await _context.UserStageProgresses
                .FirstOrDefaultAsync(p => p.UserId == userId && p.StageId == previousStage.Id);

            return progress != null && progress.IsLearnCompleted && progress.IsQuizCompleted;
        }



        public async Task MarkLearnCompletedAsync(string userId, int stageId)
        {
            var progress = await _context.UserStageProgresses
                .FirstOrDefaultAsync(p => p.UserId == userId && p.StageId == stageId);

            if (progress == null)
            {
                progress = new UserStageProgress
                {
                    UserId = userId,
                    StageId = stageId,
                    IsLearnCompleted = true
                };

                _context.UserStageProgresses.Add(progress);
            } else
            {
                progress.IsLearnCompleted = true;
            }

            await _context.SaveChangesAsync();
        }



        public async Task MarkQuizCompletedAsync(string userId, int stageId, Dictionary<int, string>? quizAnswer)
        {
            if (quizAnswer == null || !quizAnswer.Any()) return;

            foreach (var kvp in quizAnswer)
            {
                var answer = new UserQuizAnswer
                {
                    UserId = userId,
                    QuizId = kvp.Key,
                    SelectedOption = kvp.Value
                };

                _context.UserQuizAnswers.Add(answer);
            }

            var progress = await _context.UserStageProgresses.FirstOrDefaultAsync(p => p.UserId == userId && p.StageId == stageId);

            if (progress == null)
            {
                progress = new UserStageProgress
                {
                    UserId = userId,
                    StageId = stageId,
                    IsLearnCompleted = true,
                    IsQuizCompleted = true,
                    CompletedAt = DateTime.UtcNow
                };

                _context.UserStageProgresses.Add(progress);
            } else
            {
                progress.IsQuizCompleted = true;
                if (progress.IsLearnCompleted) progress.CompletedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
        }



        public async Task<bool> IsStageCompletedAsync(string userId, int stageId)
        {
            var progress = await _context.UserStageProgresses.FirstOrDefaultAsync(p => p.UserId == userId && p.StageId == stageId);

            return progress != null && progress.IsQuizCompleted && progress.IsLearnCompleted;
        }



        public async Task<bool> ResetQuizProgressWithLimitAsync(string userId)
        {
            var progresses = await _context.UserStageProgresses
                .Where(p => p.UserId == userId)
                .ToListAsync();

            if (!progresses.Any()) return false;

            var anyCooldown = progresses.Any(p => 
                p.RetryCount >= 3 &&
                p.LastRetryAt.HasValue &&
                DateTime.UtcNow < p.LastRetryAt.Value.AddHours(2)
            );
            
            if (anyCooldown) return false;

            foreach (var p in progresses)
            {
                if (p.LastRetryAt.HasValue && DateTime.UtcNow >= p.LastRetryAt.Value.AddHours(2)) p.RetryCount = 0;

                var answers = _context.UserQuizAnswers.Where(a => a.UserId == userId && a.QuizId == p.StageId);
                _context.UserQuizAnswers.RemoveRange(answers);

                p.IsQuizCompleted = false;
                p.CompletedAt = null;
                p.RetryCount += 1;
                p.LastRetryAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            return true;
        }
    }
}