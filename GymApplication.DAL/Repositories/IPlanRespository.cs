using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymApplication.DAL.Data.Models;



namespace GymApplication.DAL.Repositories
{
    public interface IPlanRespository
    {
        Task<IEnumerable<Plan>> GetAllAsync();
        Task<Plan?>GetByIdAsync(int id);

        void Add(Plan p);
        Task UpdateAsync(Plan p);
        void Delete(Plan p);

        Task<int> SaveChangesAsync() ;
        
    }
}
