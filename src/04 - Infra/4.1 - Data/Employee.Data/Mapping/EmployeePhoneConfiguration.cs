using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Data.Mapping
{
    public class EmployeePhoneConfiguration : IEntityTypeConfiguration<Employee.Domain.Entities.Phone>
    {
        public void Configure(EntityTypeBuilder<Employee.Domain.Entities.Phone> builder)
        {
            builder.ToTable("Phones");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Number)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(p => p.Type)
                .IsRequired()
                .HasConversion<int>(); 

            builder.Property(p => p.IsPrimary)
                .IsRequired();

            builder.HasOne(p => p.Employee)
                .WithMany(e => e.Phones)
                .HasForeignKey(p => p.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(p => p.EmployeeId);
        }
    }
}
