namespace Ensek.Persistence.Configurations
{
  using Domain.Entities;
  using Microsoft.EntityFrameworkCore;
  using Microsoft.EntityFrameworkCore.Metadata.Builders;

  public class MeterReadingConfiguration : IEntityTypeConfiguration<MeterReading>
  {
    public void Configure(EntityTypeBuilder<MeterReading> builder)
    {
      builder.Property(e => e.Id);
      builder.Property(e => e.RecordedAt).IsRequired();
      builder.Property(e => e.Value).IsRequired();
      builder.Property(e => e.AccountId).IsRequired();
    }
  }
}