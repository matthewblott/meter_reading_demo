namespace Ensek.Infrastructure.Files
{
  using System;
  using System.Collections.Generic;
  using System.Globalization;
  using System.IO;
  using System.Linq;
  using Application.Common.Interfaces;
  using Application.Products.Commands;
  using CsvHelper;
  using Microsoft.AspNetCore.Http;

  public class CsvFileReader : ICsvFileReader
  {
    public IEnumerable<Import.Model> ReadProductsFile(IFormFile file)
    {
      using (var stream = file.OpenReadStream())
      {
        using (var reader = new StreamReader(stream))
        {
          using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
          {
            csv.Configuration.HasHeaderRecord = true;
            csv.Configuration.RegisterClassMap<ProductFileRecordMap>();
      
            var records = csv.GetRecords<Import.Model>();

            var list = records.ToList();
            
            return list;
            
          }
        
        }
        
      }
      
    }
    
  }
  
}