using Ensek.Common;

namespace Ensek.WebUI.Services
{
  using Microsoft.Extensions.DependencyInjection;
  using Ensek.Common;

  public static class ServiceCollectionCurrentUserServiceExtensions
  {
    public static void AddCurrentUserService(this IServiceCollection services)
    {
      services.AddScoped<ICurrentUserService, CurrentUserService>();
    }
    
  }
  
}