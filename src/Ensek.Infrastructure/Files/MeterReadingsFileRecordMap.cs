namespace Ensek.Infrastructure.Files
{
  using Application.MeterReadings.Commands;
  using CsvHelper.Configuration;

  public sealed class MeterReadingsFileRecordMap : ClassMap<Import.Model.CsvRow>
  {
    public MeterReadingsFileRecordMap()
    {
      Map(m => m.AccountId).Name("AccountId");
      Map(m => m.RecordedAt).Name("MeterReadingDateTime");
      Map(m => m.Value).Name("MeterReadValue");
    }
    
  }
  
}