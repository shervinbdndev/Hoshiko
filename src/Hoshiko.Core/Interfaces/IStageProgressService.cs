using Hoshiko.Domain.Entities;

namespace Hoshiko.Core.Interfaces
{
    public interface IStageProgressService
    {
        Task<Stage?> GetNextStageForUserAsync(string userId);
        Task<bool> CanAccessStageAsync(string userId, int stageId);
        Task MarkLearnCompletedAsync(string userId, int stageId);
        Task MarkQuizCompletedAsync(string userId, int stageId, Dictionary<int, string> quizAnswers);
    }
}