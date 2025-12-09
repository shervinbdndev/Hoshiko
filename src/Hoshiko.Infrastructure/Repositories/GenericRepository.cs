using System.Linq.Expressions;
using Hoshiko.Core.Interfaces;
using Hoshiko.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hoshiko.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;


        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }



        public async Task<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);
        public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();
        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate) => await _dbSet.Where(predicate).ToListAsync();
        public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);
        public void Update(T entity) => _dbSet.Update(entity);
        public void Remove(T entity) => _dbSet.Remove(entity);
    }
}