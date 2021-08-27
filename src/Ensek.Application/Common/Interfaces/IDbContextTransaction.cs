namespace Ensek.Application.Common.Interfaces
{
  using System.Threading.Tasks;

  public interface IDbContextTransaction
  {
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    void RollbackTransaction();
  }
}