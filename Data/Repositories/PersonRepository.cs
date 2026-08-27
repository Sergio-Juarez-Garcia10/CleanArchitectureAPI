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
    public class PersonRepository :IRepository<PersonEntity, Guid>, ICodeRepository<PersonEntity>
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public PersonRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<PersonEntity?> GetByIdAsync(Guid id)
        {
            return await _applicationDbContext.Persons
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<PersonEntity>> GetAllAsync()
        {
            return await _applicationDbContext.Persons
                .AsNoTracking()
                .OrderBy(p => p.FirstName)
                .ThenBy(p => p.LastName)
                .ToListAsync();
        }

        public async Task AddAsync(PersonEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            await _applicationDbContext.Persons.AddAsync(entity);

        }

        public Task UpdateAsync(PersonEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _applicationDbContext.Persons.Update(entity);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(PersonEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _applicationDbContext.Persons.Remove(entity);

            return Task.CompletedTask;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _applicationDbContext.SaveChangesAsync();
        }

        //ICodeRepository
        public async Task<PersonEntity?> GetByCodeAsync(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentException("El codigo no puede estar vacion", nameof(code));
            }

            var normalizedCode = code.ToUpperInvariant();
            return await _applicationDbContext.Persons.FirstOrDefaultAsync(p => p.Code == normalizedCode);
        }

        public async Task<bool> ExistsWithCodeAsync(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentException("El codigo no puede estar vacion", nameof(code));
            }

            var normalizedCode = code.ToUpperInvariant();

            return await _applicationDbContext.Persons.AnyAsync(p => p.Code == normalizedCode);
        }
    }
}
