namespace Ensek.Application.Employees.Commands
{
  using System;
  using System.Threading;
  using System.Threading.Tasks;
  using AutoMapper;
  using Common.Interfaces;
  using FluentValidation;
  using MediatR;

  public class Update
  {
    // Command
    public class Command : IRequest<Unit>
    {
      [IgnoreMap]
      public int? Id { get; set; }
      public string FirstName { get; set; }
      public string LastName { get; set; }
    }
    
    // Validator
    public class Validator : AbstractValidator<Command>
    {
      public Validator()
      {
        RuleFor(v => v.Id).NotEmpty();
        RuleFor(e => e.FirstName).Length(10).NotEmpty();
        RuleFor(e => e.LastName).Length(20).NotEmpty();
      }
    }
    
    // Handler
    public class Handler : IRequestHandler<Command>
    {
      private readonly IEnsekDbContext _db;

      public Handler(IEnsekDbContext db)
      {
        _db = db;
      }

      public async Task<Unit> Handle(Command command, CancellationToken token)
      {
        if (!command.Id.HasValue)
        {
          throw new NullReferenceException();
        }

        var entity = await _db.Employees.FindAsync(command.Id.Value);

        entity.FirstName = command.FirstName;
        entity.LastName = command.LastName;
        
        await _db.SaveChangesAsync(token);

        return Unit.Value;
        
      }
      
    }

  }
  
}