using GymApplication.DAL.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymApplication.DAL.Data.Models
{
    public class Trainer : GymUser
    {
        //public DateTime HireDate { get; set; } => CreatedAt
        public Speciality Speciality { get; set; }
        public ICollection<Session>Sessions { get; set; }
    }
}
