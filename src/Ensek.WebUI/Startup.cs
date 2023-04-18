[assembly: JetBrains.Annotations.AspMvcViewLocationFormat(@"~\Features\{1}\{0}.cshtml")]
[assembly: JetBrains.Annotations.AspMvcViewLocationFormat(@"~\Features\{0}.cshtml")]
[assembly: JetBrains.Annotations.AspMvcViewLocationFormat(@"~\Features\Shared\{0}.cshtml")]
[assembly: JetBrains.Annotations.AspMvcPartialViewLocationFormat(@"~\Features\Shared\{0}.cshtml")]

namespace Ensek.WebUI
{
  using System;
  using Application;
  using Application.Common.Interfaces;
  using Filters;
  using FluentValidation.AspNetCore;
  using Microsoft.AspNetCore.Builder;
  using Microsoft.AspNetCore.Hosting;
  using Microsoft.Extensions.Configuration;
  using Microsoft.Extensions.DependencyInjection;
  using Infrastructure;
  using Persistence;
  using Services;

  public class Startup
  {
    public Startup(IConfiguration configuration, IWebHostEnvironment environment)
    {
      Configuration = configuration;
      Environment = environment;
    }

    public IConfiguration Configuration { get; }
    public IWebHostEnvironment Environment { get; }
   
    public void ConfigureServices(IServiceCollection services)
    {
      var connectionString = Configuration.GetConnectionString("EnsekDatabase");

      if (string.IsNullOrEmpty(connectionString))
      {
        throw new NullReferenceException();
      }
      
      services.AddInfrastructure(Configuration, Environment);
      services.AddPersistence(connectionString);
      services.AddApplication();
      services.AddUrlHelper(); 
      services.AddHttpContextAccessor();
      services.AddControllersWithViews(options => options.Filters.Add(typeof(DbContextTransactionFilter)))
        .AddNewtonsoftJson()
        .AddFeatureFolders()
        .AddFluentValidation(fv =>
        {
          fv.RegisterValidatorsFromAssemblyContaining<IEnsekDbContext>();
        });

      services.AddMvc();
      services.AddRouting(option => option.LowercaseUrls = true);

    }

    public void Configure(IApplicationBuilder app)
    {
      app.UseDeveloperExceptionPage();
      app.UseStaticFiles();
      app.UseRouting();
      app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());
    }
    
  }
  
}