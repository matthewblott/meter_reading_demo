namespace Ensek.Application.MeterReadings.Commands
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
    public class Command : IRequest<Unit>
    {
      [IgnoreMap]
      public int Id { get; set; }
      public int AccountId { get; set; }
      public string RecordedAt { get; set; }
      public short Value { get; set; }

    }

    // Validator
    public class Validator : AbstractValidator<Command>
    {
      public Validator()
      {
        // RuleFor(v => v.Id) ...
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
        var entity = await _db.MeterReadings.FindAsync(command.Id);

        entity.RecordedAt = command.RecordedAt;
        entity.AccountId = command.AccountId;
        entity.Value = command.Value;
        
        await _db.SaveChangesAsync(token);

        return Unit.Value;
        
      }
      
    }

  }
  
}