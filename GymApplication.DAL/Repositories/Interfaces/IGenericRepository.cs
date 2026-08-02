using GymApplication.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymApplication.DAL.Repositories.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity :  BaseEntity , new()
    {
        Task<TEntity?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<IEnumerable<TEntity>> GetAllAsync(bool tracking = false , CancellationToken ct = default);

        Task<bool>AnyAsync(Expression<Func<TEntity , bool>>Predicate , CancellationToken ct);

        Task<TEntity?>FirstOrDefaultAsync(Expression<Func<TEntity , bool>>predicate,bool tracking = false , CancellationToken ct = default);

        void Add(TEntity entity);
        void Update(TEntity entity); 
        void Delete(TEntity entity);
    }
}
