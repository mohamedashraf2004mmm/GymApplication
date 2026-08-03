using GymApplication.DAL.Data.DbContextss;
using GymApplication.DAL.Data.Models;
using GymApplication.DAL.Repositories.Interfaces;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GymApplication.DAL.Repositories.Classes
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GymDbContext dbContext;
        private readonly Dictionary<string, object> _repositories =[]; 

        public UnitOfWork(GymDbContext dbContext, ISessionRepository sessionRepository)
        {
            this.dbContext = dbContext;
            SessionRepository = sessionRepository;
        }

        public ISessionRepository SessionRepository { get; }

        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            //check TEntity
            var typeName = typeof(TEntity).Name;
            if (_repositories.TryGetValue(typeName , out object? value))
            {
                return (IGenericRepository<TEntity>)value;
            }
            else
            {
                var repo = new GenericRepository<TEntity>(dbContext);
                _repositories[typeName] = repo;
                return repo;
            }
        }

        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
           return await dbContext.SaveChangesAsync(ct);
        }
    }
}
