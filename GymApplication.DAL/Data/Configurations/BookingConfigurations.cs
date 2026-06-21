using GymApplication.DAL.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymApplication.DAL.Data.Configurations
{
    public class BookingConfigurations : IEntityTypeConfiguration<Booking>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Booking> builder)
        {
            builder.Ignore(e => e.Id);

            builder.Property(e => e.CreatedAt)
                   .HasColumnName("BookingDate")
                   .HasDefaultValueSql("GETDATE()");

            builder.HasKey(e => new
            {
                e.SessionId , e.MemberId
            });
        }
    }
}
