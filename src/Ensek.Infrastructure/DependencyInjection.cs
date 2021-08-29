namespace Ensek.Infrastructure
{
  using Application.Common.Interfaces;
  using Files;
  using Microsoft.AspNetCore.Hosting;
  using Microsoft.Extensions.Configuration;
  using Microsoft.Extensions.DependencyInjection;

  public static class DependencyInjection
  {
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, 
      IConfiguration configuration, IWebHostEnvironment environment)
    {
      services.AddScoped<ICsvFileReader, CsvFileReader>();

      return services;

    }
    
  }
  
}