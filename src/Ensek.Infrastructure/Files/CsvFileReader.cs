namespace Ensek.Infrastructure.Files
{
  using System.Collections.Generic;
  using System.Globalization;
  using System.IO;
  using Application.Common.Interfaces;
  using Application.MeterReadings.Commands;
  using CsvHelper;
  using Microsoft.AspNetCore.Http;

  public class CsvFileReader : ICsvFileReader
  {
    public IEnumerable<Import.Model.CsvRow> ReadMeterReadingsFile(IFormFile file)
    {
      using var stream = file.OpenReadStream();
      using var reader = new StreamReader(stream);
      using var csv = new CsvReader(reader, new CultureInfo("en-gb"));
      
      csv.Configuration.HasHeaderRecord = true;
      csv.Configuration.RegisterClassMap<MeterReadingsFileRecordMap>();
      csv.Configuration.ReadingExceptionOccurred = ex => false;
      csv.Configuration.PrepareHeaderForMatch = (header, index) => header.ToLower();
      csv.Read();
      csv.ReadHeader();
      csv.ValidateHeader<Import.Model.CsvRow>();
      
      var list = new List<Import.Model.CsvRow>();
    
      var rowNumber = 0;
      
      while (csv.Read())
      {
        rowNumber++;
        
        var record = csv.GetRecord<Import.Model.CsvRow>();
    
        if (record == null)
        {
          record = new Import.Model.CsvRow();
        }
        else
        {
          record.IsValid = true;
        }
    
        record.RowNumber = rowNumber;
        
        list.Add(record);
    
      }
    
      return list;
    }
    
  }
  
}