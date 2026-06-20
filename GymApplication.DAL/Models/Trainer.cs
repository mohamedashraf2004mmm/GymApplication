using GymApplication.DAL.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymApplication.DAL.Models
{
    public class Trainer : GymUser
    {
        //public DateTime HireDate { get; set; } => CreatedAt
        public Speciality Speciality { get; set; }
    }
}
