using GymApplication.DAL.Models;

namespace GymApplication.Models
{
    public class Plan : BaseEntity
    {
        
        public string PlanName { get; set; } = string.Empty;
        public string PlanDescription { get; set; } = string.Empty;
        public int DurationDays {  get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
       
    }
}
