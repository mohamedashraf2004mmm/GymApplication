using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Microsoft.EntityFrameworkCore;
using GymApplication.DAL.Data.Models;


namespace GymApplication.DAL.Data.Configurations
{
    public class PlanConfiguration : IEntityTypeConfiguration<Plan>
    {
        public void Configure(EntityTypeBuilder<Plan> builder)
        {
            builder.Property(x => x.PlanName)
                 .HasColumnName("Name")
                 .HasMaxLength(50);

            builder.Property(x => x.PlanDescription)
                .HasMaxLength(200);

            builder.Property(x => x.Price)
                .HasPrecision(10, 2);

            builder.Property(x => x.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            builder.ToTable(tb =>
            {
                tb.HasCheckConstraint("PlanDurationCheck", "DurationDays between 1 and 365");
            });

        }
    }
}
