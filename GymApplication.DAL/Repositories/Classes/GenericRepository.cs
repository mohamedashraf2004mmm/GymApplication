using GymApplication.DAL.Data.Models;
using GymApplication.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymApplication.DAL.Repositories.Classes
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity, new()
    {
        public Task<int> AddAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> GetAllById(bool tracking = false, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
