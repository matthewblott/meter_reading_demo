namespace Ensek.Application.Accounts.Commands
{
  using System.Threading;
  using System.Threading.Tasks;
  using Common.Interfaces;
  using FluentValidation;
  using MediatR;

  public class Delete
  {
    // Query
    
    // Validator
    
    // Handler
    
    // Command
    public class Command : IRequest
    {
      public int Id { get; set; }
    }
    
    public class CommandValidator : AbstractValidator<Command>
    {
      public CommandValidator()
      {
        // RuleFor(v => v.Id) ...
      }
    }
    
    // Handler
    public class CommandHandler : IRequestHandler<Command>
    {
      private readonly IEnsekDbContext _db;

      public CommandHandler(IEnsekDbContext db)
      {
        _db = db;
      }

      public async Task<Unit> Handle(Command command, CancellationToken token)
      {
        var entity = await _db.Accounts
          .FindAsync(command.Id);

        _db.Accounts.Remove(entity);

        await _db.SaveChangesAsync(token);

        return Unit.Value;
      }
    }
    
  }
  
}