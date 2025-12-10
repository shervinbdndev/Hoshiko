using Hoshiko.Core.Interfaces;
using Hoshiko.Domain.Entities;
using Hoshiko.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hoshiko.Infrastructure.Repositories
{
    public class StageRepository : GenericRepository<Stage>, IStageRepository
    {
        private readonly ApplicationDbContext _context;

        public StageRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }


        public async Task<Stage?> GetWithQuizzesAsync(int id)
        {
            return await _context.Stages
                .Include(s => s.Quizzes)
                .Include(s => s.Learns)
                .FirstOrDefaultAsync(s => s.Id == id);
        }
    }
}