namespace GymApplication.DAL.Data.Models
{
    public class Plan : BaseEntity
    {
        
        public string PlanName { get; set; } = string.Empty;
        public string PlanDescription { get; set; } = string.Empty;
        public int DurationDays {  get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }

        #region Relationships
        public ICollection<MemberShip> PlanMembers { get; set; } = default!;
        #endregion

    }
}
