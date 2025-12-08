using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Data.Mapping
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee.Domain.Entities.Employee>
    {
        public void Configure(EntityTypeBuilder<Employee.Domain.Entities.Employee> builder)
        {
            builder.ToTable("Employees");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(200);
            builder.Property(e => e.DocumentNumber)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(e => e.Phone)
                .IsRequired()
                .HasMaxLength(20);
            builder.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(200);
            builder.Property(e => e.Role)
                .IsRequired();
            builder.Property(e => e.Status)
                .IsRequired();
            builder.Property(e => e.CreatedAt)
                .IsRequired();
            builder.Property(e => e.UpdatedAt);
            builder.HasOne(e => e.Manager)
                .WithMany()
                .HasForeignKey(e => e.ManagerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
