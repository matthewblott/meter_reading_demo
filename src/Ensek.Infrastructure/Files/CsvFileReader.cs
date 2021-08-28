using System.Linq;

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
    // public IEnumerable<Import.Model> ReadMeterReadingsFile(IFormFile file)
    // {
    //   using (var stream = file.OpenReadStream())
    //   {
    //     using (var reader = new StreamReader(stream))
    //     {
    //       using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
    //       {
    //         csv.Configuration.HasHeaderRecord = true;
    //         csv.Configuration.RegisterClassMap<MeterReadingsFileRecordMap>();
    //         csv.Configuration.ReadingExceptionOccurred = ex => false;
    //
    //         var records = csv.GetRecords<Import.Model>();
    //
    //         var list = records.ToList();
    //
    //         return list;
    //       }
    //
    //     }
    //
    //   }
    // }

    public IEnumerable<Import.Model> ReadMeterReadingsFile(IFormFile file)
    {
      using var stream = file.OpenReadStream();
      using var reader = new StreamReader(stream);
      using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
      
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
    
  }
  
}