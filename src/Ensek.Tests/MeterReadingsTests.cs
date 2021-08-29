namespace Ensek.Tests
{
  using System.Data.Common;
  using System.IO;
  using System.Linq;
  using System.Threading;
  using System.Threading.Tasks;
  using Application.Common.Interfaces;
  using AutoMapper;
  using Infrastructure.Files;
  using Microsoft.AspNetCore.Http;
  using Microsoft.Data.Sqlite;
  using Microsoft.EntityFrameworkCore;
  using Microsoft.Extensions.Configuration;
  using Microsoft.Extensions.DependencyInjection;
  using NUnit.Framework;
  using Persistence;
  using WebUI;

  public class MeterReadingsTests
  {
    private static DbConnection CreateInMemoryDatabase()
    {
      var connection = new SqliteConnection("Filename=:memory:");

      connection.Open();

      using var cmd = connection.CreateCommand();
      cmd.CommandText = File.ReadAllText("Scripts/Accounts.sql");
      cmd.ExecuteNonQuery();
      cmd.CommandText = File.ReadAllText("Scripts/MeterReadings.sql");
      cmd.ExecuteNonQuery();

      return connection;
    }

    [Test]
    public async Task Should_be_able_to_upload_meter_readings_file_and_print_results()
    {
      var host = new HostEnvironment();
      var services = new ServiceCollection();
      
      IConfigurationRoot configuration = new ConfigurationBuilder()
        .Build();
      
      var startup = new Startup(configuration, host);

      startup.ConfigureServices(services);

      var provider = services.BuildServiceProvider();
      
      var descriptor = services.SingleOrDefault(
        d => d.ServiceType ==
             typeof(DbContextOptions<EnsekDbContext>));
      
      if (descriptor != null)
      {
        services.Remove(descriptor);
      }

      services.AddDbContext<EnsekDbContext>(options =>
      {
        options.UseSqlite(CreateInMemoryDatabase());
      });

      DbContextOptionsBuilder<EnsekDbContext> builder = new DbContextOptionsBuilder<EnsekDbContext>();

      var options = builder.UseSqlite(CreateInMemoryDatabase()).Options;
      var db = new EnsekDbContext(options);
      var mapper = provider.GetService<IMapper>()!;

      ICsvFileReader fileReader = new CsvFileReader();
      
      var handler = new Application.MeterReadings.Commands.Import.Handler(db, mapper, fileReader);
      var token = new CancellationToken();
      var command = new Application.MeterReadings.Commands.Import.Command();

      const string filePath = "Documents/meter_readings.csv";

      await using var stream = new MemoryStream((await File.ReadAllBytesAsync(filePath, token)).ToArray());
      var formFile = new FormFile(stream, 0, stream.Length, "streamFile","meter_readings.csv");

      command.File = formFile;
      
      await handler.Handle(command, token);
 
      const int expectedNumberOfNewDatabaseRecords = 24;
      
      var count = await db.MeterReadings.CountAsync(cancellationToken: token);

      Assert.True(count == expectedNumberOfNewDatabaseRecords);
      
    }
    
  }
  
}