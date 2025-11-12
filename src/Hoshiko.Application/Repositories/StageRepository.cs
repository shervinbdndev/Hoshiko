using Hoshiko.Data.Contexts;
using Hoshiko.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hoshiko.Application.Repositories;

public class StageRepository : Repository<Stage>, IStageRepository
{
    public StageRepository(AppDbContext context) : base(context) { }
    
    public async Task<Stage?> GetStageWithSectionAsync(int id)
    {
        return await _context.Stages
            .Include(s => s.Sections)
            .FirstOrDefaultAsync(s => s.Id == id);
    }
}