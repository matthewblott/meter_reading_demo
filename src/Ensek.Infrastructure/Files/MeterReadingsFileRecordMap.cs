namespace Ensek.Infrastructure.Files
{
  using Application.MeterReadings.Commands;
  using CsvHelper.Configuration;

  public sealed class MeterReadingsFileRecordMap : ClassMap<Import.Model>
  {
    public MeterReadingsFileRecordMap()
    {
      Map(m => m.AccountId).Name("AccountId");
      Map(m => m.RecordedAt).Name("MeterReadingDateTime");
      Map(m => m.Value).Name("MeterReadValue");
    }
    
  }
  
}