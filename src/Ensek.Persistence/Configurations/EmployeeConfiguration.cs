namespace Ensek.Persistence.Configurations
{
  using Domain.Entities;
  using Microsoft.EntityFrameworkCore;
  using Microsoft.EntityFrameworkCore.Metadata.Builders;

  public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
  {
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
      builder.Property(e => e.EmployeeId);
      builder.Property(e => e.FirstName).IsRequired().HasMaxLength(10);
      builder.Property(e => e.LastName).IsRequired().HasMaxLength(20);

      // builder.HasOne(d => d.Manager)
      //   .WithMany(p => p.DirectReports);
        // .HasForeignKey(d => d.ReportsTo);
      
    }
  }
}