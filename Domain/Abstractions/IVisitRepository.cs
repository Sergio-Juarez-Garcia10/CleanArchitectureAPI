using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstractions
{
    public interface IVisitRepository<TEntity> where TEntity : class
    {
        Task<bool> HasActiveVisitAsync(Guid personId);

        Task<TEntity?> GetActiveVisitByPersonCodeAsync(string personCode);

        Task<IEnumerable<TEntity>> GetActiveVisitsAsync();

        Task<IEnumerable<TEntity>> GetVisitsByPersonIdAsync(Guid personId);
    }
}
