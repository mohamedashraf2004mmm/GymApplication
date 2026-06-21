using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymApplication.DAL.Data.Models
{
    public class Booking : BaseEntity
    {
        //Member and session
        public Member Member { get; set; } = default!;
        public int MemberId { get; set; }

        public Session Session { get; set; }
        public int SessionId { get; set; }

        //public DateTime BookingDate { get; set; } ==> CreatedAt

        public bool IsAttended { get; set; }
    }
}
