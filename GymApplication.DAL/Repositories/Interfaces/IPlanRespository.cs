using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymApplication.DAL.Data.Models;



namespace GymApplication.DAL.Repositories.Interfaces
{
    public interface IPlanRespository : IGenericRepository<Plan> 
    {
        Task<IEnumerable<Plan>> GetPlanWithMembers();
    }
}
