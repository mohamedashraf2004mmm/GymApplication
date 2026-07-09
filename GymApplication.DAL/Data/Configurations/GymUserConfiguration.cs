using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GymApplication.DAL.Data.Models;

namespace GymApplication.DAL.Data.Configurations
{
    public class GymUserConfiguration<T> : IEntityTypeConfiguration<T> where T : GymUser
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(x => x.name)
                   .HasColumnType("varchar")
                   .HasMaxLength(50);

            builder.HasIndex(x => x.email).IsUnique();
            builder.HasIndex(x => x.name).IsUnique();

            builder.ToTable(tb =>
            {
                tb.HasCheckConstraint("EmailCheck", "Email LIKE '%@%.%'");
                tb.HasCheckConstraint("PhoneCheck", "Phone LIKE '010________'");
            });

            builder.OwnsOne(x => x.Address, address =>
            {
                address.Property(x=>x.Street).HasColumnName("Street")
                                             .HasColumnType("varchar")
                                             .HasMaxLength(30);


                address.Property(x => x.City).HasColumnName("City")
                                             .HasColumnType("varchar")
                                             .HasMaxLength(30);
            });
        }
    }
}
