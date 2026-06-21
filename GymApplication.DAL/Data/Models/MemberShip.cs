using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymApplication.DAL.Data.Models
{
    public class MemberShip : BaseEntity
    {
        public Plan Plan { get; set; }
        public int PlanId { get; set; }

        public Member Member { get; set; }
        public int MemberId { get; set; }

      //  public DateTime StartDate { get; set; } we already have CreatedAt
        public DateTime EndDate { get; set; }

        public string Status => EndDate > DateTime.Now ? "Active" : "Expired";

        public bool IsActive => EndDate > DateTime.Now;
    }
}
