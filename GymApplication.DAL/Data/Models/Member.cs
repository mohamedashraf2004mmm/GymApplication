using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymApplication.DAL.Data.Models
{
    public class Member : GymUser
    {
        public string? Photo { get; set; }

        // public DateTime JoinDate { get; set; } => CreatedAt(BAseEntity)

        #region relationships
        public HealthRecord HealthRecord { get; set; }
        public ICollection<MemberShip> MemberShips { get; set; } = default!;

        public ICollection<Booking> MemberSessions { get; set; }
        #endregion
    }
}
