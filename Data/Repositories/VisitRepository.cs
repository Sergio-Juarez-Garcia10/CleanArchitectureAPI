using Data.Persistence;
using Domain;
using Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class VisitRepository : IRepository<VisitEntity, Guid>, IVisitRepository<VisitEntity>
    {

        private readonly ApplicationDbContext _context;
        public VisitRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<VisitEntity?> GetByIdAsync(Guid id)
        {
            return await _context.Visits
                .Include(x => x.Person)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<VisitEntity>> GetAllAsync()
        {
            return await _context.Visits
                .Include(x => x.Person)
                .AsNoTracking()
                .OrderByDescending(x => x.EntryTime)
                .ToListAsync();
        }

        public async Task AddAsync(VisitEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            await _context.Visits.AddAsync(entity);
        }

        public Task UpdateAsync(VisitEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            _context.Visits.Update(entity);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(VisitEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            _context.Visits.Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        // Implement IvisitRepository 

        public async Task<bool> HasActiveVisitAsync(Guid personId)
        {
            return await _context.Visits
                .AnyAsync(v => v.PersonId == personId && v.ExitTime == null);
        }

        public async Task<VisitEntity?> GetActiveVisitByPersonCodeAsync(string personCode)
        {
            return await _context.Visits
                .Include(v => v.Person)
                .FirstOrDefaultAsync(v => v.Person != null 
                && v.Person.Code.ToUpper() == personCode.ToUpper() && v.ExitTime == null);
        }

        public async Task<IEnumerable<VisitEntity>> GetActiveVisitsAsync()
        {
            return await _context.Visits
                .Include(v => v.Person)
                .Where(v => v.ExitTime == null)
                .OrderBy(v => v.EntryTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<VisitEntity>> GetVisitsByPersonIdAsync(Guid personId)
        {
            return await _context.Visits
                .Include(v => v.Person)
                .Where(v => v.PersonId == personId)
                .OrderByDescending(v => v.EntryTime)
                .ToListAsync();
        }
    }
}
