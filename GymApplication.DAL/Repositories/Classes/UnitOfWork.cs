using GymApplication.DAL.Data.DbContextss;
using GymApplication.DAL.Data.Models;
using GymApplication.DAL.Repositories.Interfaces;
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

        public UnitOfWork(GymDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
        {
           //check TEntity!!
           var typeName = typeof(TEntity).Name;
            //if (_repositories.ContainsKey(typeName)) 
        }

        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
           return await dbContext.SaveChangesAsync(ct);
        }
    }
}
