namespace Ensek.Common
{
  public interface ICurrentUserService
  {
    string UserId { get; }

    bool IsAuthenticated { get; }
  }
}