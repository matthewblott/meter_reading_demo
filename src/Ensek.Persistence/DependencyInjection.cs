namespace Ensek.Persistence
{
  using Application.Common.Interfaces;
  using Microsoft.EntityFrameworkCore;
  using Microsoft.Extensions.DependencyInjection;

  public static class DependencyInjection
  {
    public static IServiceCollection AddPersistence(this IServiceCollection services, string connectionString)
    {
      services.AddDbContext<EnsekDbContext>(options =>
      {
        var enabled = false;
#if DEBUG
        enabled = true;
#endif
        // ReSharper disable once ConditionIsAlwaysTrueOrFalse
        options.UseSqlite(connectionString)
          .EnableSensitiveDataLogging(enabled);
      });

      services.AddScoped<IEnsekDbContext>(provider => provider.GetRequiredService<EnsekDbContext>());
      services.AddScoped<IDbContextTransaction>(provider => provider.GetRequiredService<EnsekDbContext>());
      
      return services;
      
    }
    
  }
  
}