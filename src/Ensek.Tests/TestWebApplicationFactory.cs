using System.Data.Common;
using System.Linq;
using Ensek.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Ensek.Tests
{
  using Microsoft.AspNetCore.Hosting;
  using Microsoft.AspNetCore.Mvc.Authorization;
  using Microsoft.AspNetCore.Mvc.Testing;
  using Microsoft.AspNetCore.TestHost;
  using Microsoft.Extensions.DependencyInjection;
  using WebUI;
  using WebUI.Filters;

  public class TestWebApplicationFactory : WebApplicationFactory<Startup>
  {
    private static DbConnection CreateInMemoryDatabase()
    {
      var connection = new SqliteConnection("Filename=:memory:");

      connection.Open();

      return connection;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
      var factory = WithWebHostBuilder(hostBuilder =>
      {
        hostBuilder.ConfigureServices(services =>
        {
          services.AddMvc(options =>
            {
              options.Filters.Add(new AllowAnonymousFilter());
              options.Filters.Add(new AllowAnonymousFilter());

              var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbContextOptions<EnsekDbContext>));

              if (descriptor != null)
              {
                services.Remove(descriptor);
              }

              services.AddDbContext<EnsekDbContext>(options => { options.UseSqlite(CreateInMemoryDatabase()); });
            })
            .AddApplicationPart(typeof(Startup).Assembly);
        });
      });
    }
  }
}