namespace Ensek.Application.Common.Interfaces
{
  using System.Threading;
  using System.Threading.Tasks;
  using Domain.Entities;
  using Microsoft.EntityFrameworkCore;

  public interface IEnsekDbContext
  {
    DbSet<Employee> Employees { get; set; }
    DbSet<EmployeeTerritory> EmployeeTerritories { get; set; }
    DbSet<Role> Groups { get; set; }
    DbSet<Product> Products { get; set; }
    DbSet<Territory> Territories { get; set; }
    DbSet<User> Users { get; set; }
    DbSet<UserRole> UserGroups { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);

  }
  
}