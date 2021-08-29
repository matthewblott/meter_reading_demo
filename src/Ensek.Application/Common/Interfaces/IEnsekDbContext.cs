namespace Ensek.Application.Common.Interfaces
{
  using System.Threading;
  using System.Threading.Tasks;
  using Domain.Entities;
  using Microsoft.EntityFrameworkCore;

  public interface IEnsekDbContext
  {
    DbSet<Account> Accounts { get; set; }
    DbSet<MeterReading> MeterReadings { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);

  }
  
}