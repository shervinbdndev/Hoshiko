using Hoshiko.Domain.Entities;

namespace Hoshiko.Core.Interfaces
{
    public interface IStageRepository : IGenericRepository<Stage>
    {
        Task<Stage?> GetWithQuizzesAsync(int id);
        Task<Stage?> GetBySlugAsync(string slug);
    }
}