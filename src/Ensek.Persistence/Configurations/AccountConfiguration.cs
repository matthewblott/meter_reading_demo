namespace Ensek.Persistence.Configurations
{
  using Domain.Entities;
  using Microsoft.EntityFrameworkCore;
  using Microsoft.EntityFrameworkCore.Metadata.Builders;

  public class AccountConfiguration : IEntityTypeConfiguration<Account>
  {
    public void Configure(EntityTypeBuilder<Account> builder)
    {
      builder.Property(e => e.Id);
      builder.Property(e => e.FirstName).IsRequired().HasMaxLength(10);
      builder.Property(e => e.LastName).IsRequired().HasMaxLength(20);

      // builder.HasOne(d => d.Manager)
      //   .WithMany(p => p.DirectReports);
        // .HasForeignKey(d => d.ReportsTo);
      
    }
  }
}