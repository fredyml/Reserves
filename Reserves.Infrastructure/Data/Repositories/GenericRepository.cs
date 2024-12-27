using Microsoft.EntityFrameworkCore;
using Reserves.Application.Interfaces;
using Reserves.Domain.Entities;
using System.Linq.Expressions;

namespace Reserves.Infrastructure.Data.Repositories
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null, bool includeRelated = false)
        {
            IQueryable<T> query = _dbSet;

            if (includeRelated)
            {
                if (typeof(T) == typeof(Reservation))
                {
                    query = query.Include(r => (r as Reservation).Space)
                                 .Include(r => (r as Reservation).User);
                }
            }

            return predicate == null
                ? await query.ToListAsync()
                : await query.Where(predicate).ToListAsync();
        }


        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
        }

        public async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task<IEnumerable<Reservation>> GetOverlappingReservationsAsync(int spaceId, DateTime startDate, DateTime endDate) { return await _context.Set<Reservation>().Where(r => r.SpaceId == spaceId && ((startDate >= r.StartDate && startDate < r.EndDate) || (endDate > r.StartDate && endDate <= r.EndDate) || (startDate < r.StartDate && endDate > r.EndDate))).ToListAsync(); }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
