using GymApplication.DAL.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymApplication.DAL.Models
{
    public abstract class GymUser : BaseEntity
    {
        public string name { get; set; }
        public string email { get; set; }
        public  DateOnly DateOfBirth { get; set; }

        public Address Address { get; set; }

        public Gender Gender { get; set; }


    }
    [Owned]
    public class Address
    {
        public string Street { get; set; }
        public string City { get; set; }
        public int BuildingNumber { get; set; }
    }
}
