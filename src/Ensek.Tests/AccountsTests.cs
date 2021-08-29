namespace Ensek.Tests
{
  using System;
  using System.Data.Common;
  using System.IO;
  using System.Linq;
  using System.Threading;
  using AutoMapper;
  using Microsoft.Data.Sqlite;
  using Microsoft.EntityFrameworkCore;
  using Microsoft.Extensions.Configuration;
  using Microsoft.Extensions.DependencyInjection;
  using NUnit.Framework;
  using Persistence;
  using WebUI;

  public class AccountsTests
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
    public void Should_return_paged_records_correctly()
    {
      var host = new HostEnvironment();
      var services = new ServiceCollection();
      
      IConfigurationRoot configuration = new ConfigurationBuilder().Build();
      
      var startup = new Startup(configuration, host);

      startup.ConfigureServices(services);

      var provider = services.BuildServiceProvider();
      
      var descriptor = services.SingleOrDefault(
        d => d.ServiceType == typeof(DbContextOptions<EnsekDbContext>));
      
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
      var handler = new Ensek.Application.Accounts.Queries.Index.Handler(db, mapper);
      var token = new CancellationToken();
      var query = new Application.Accounts.Queries.Index.Query { Page = 2 };
      var results = handler.Handle(query, token);
      var result = results.Result;
      var items = result.Items;

      var firstItem = items.First();
      var lastItem = items.Last();
      
      var expectedFirstItemFirstName = "Pam";
      var expectedLastItemFirstName = "Gladys";
      
      Assert.IsTrue(firstItem.FirstName == expectedFirstItemFirstName);
      Assert.IsTrue(lastItem.FirstName == expectedLastItemFirstName);

    }
    
  }
  
}