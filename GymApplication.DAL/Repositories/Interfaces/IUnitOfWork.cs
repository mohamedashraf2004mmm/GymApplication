using GymApplication.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymApplication.DAL.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        // public IGenericRepository<Plan> PlanRepository { get; set; } => NO

        IGenericRepository<TEntity>GetRepository<TEntity>() where TEntity : BaseEntity, new();
        Task<int> SaveChangesAsync(CancellationToken ct = default);

        public ISessionRepository SessionRepository { get; }
    }
}
