namespace Ensek.Application.Common.Interfaces
{
  using System.Collections.Generic;
  using Microsoft.AspNetCore.Http;
  using MeterReadings.Commands;

  public interface ICsvFileReader
  {
    IEnumerable<Import.Model.CsvRow> ReadMeterReadingsFile(IFormFile file);
  }
}