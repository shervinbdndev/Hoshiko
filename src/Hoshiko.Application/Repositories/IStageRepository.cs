using Hoshiko.Data.Entities;

namespace Hoshiko.Application.Repositories;

public interface IStageRepository : IRepository<Stage>
{
    Task<Stage?> GetStageWithSectionAsync(int id);
}