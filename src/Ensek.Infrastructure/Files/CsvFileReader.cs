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
    public IEnumerable<Import.Model> ReadMeterReadingsFile(IFormFile file)
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
      csv.ValidateHeader<Import.Model>();
      
      var list = new List<Import.Model>();
    
      var rowNumber = 0;
      
      while (csv.Read())
      {
        rowNumber++;
        
        var record = csv.GetRecord<Import.Model>();
    
        if (record == null)
        {
          record = new Import.Model();
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
    
    public IEnumerable<ImportApi.Model> ReadMeterReadingsFileApi(IFormFile file)
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
      
      //csv.ValidateHeader<ImportApi.Model>();
      
      var list = new List<ImportApi.Model>();
    
      var rowNumber = 0;
      
      while (csv.Read())
      {
        rowNumber++;
        
        var record = csv.GetRecord<ImportApi.Model>();
    
        if (record == null)
        {
          record = new ImportApi.Model();
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