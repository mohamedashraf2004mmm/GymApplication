using GymApplication.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymApplication.DAL.Repositories.Interfaces
{
    public interface GenericRepository<TEntity> where TEntity :  BaseEntity , new()
    {
        Task<TEntity?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<IEnumerable<TEntity>> GetAllAsync(bool tracking = false , CancellationToken ct = default);

        Task<int> AddAsync(TEntity entity);
        Task<int> UpdateAsync(TEntity entity); 
        Task<int> DeleteAsync(TEntity entity);
    }
}
