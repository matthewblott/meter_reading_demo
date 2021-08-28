namespace Ensek.Application.MeterReadings.Commands
{
  using System.Threading;
  using System.Threading.Tasks;
  using AutoMapper;
  using Common.Interfaces;
  using Domain.Entities;
  using FluentValidation;
  using MediatR;

  public class Create
  {
    public class Command : IRequest<int>
    {
      [IgnoreMap]
      public int? Id { get; set; }
      public int AccountId { get; set; }
      public string RecordedAt { get; set; }
      public short Value { get; set; }
    }

    public class Validator : AbstractValidator<Command>
    {
      public Validator()
      {
        // RuleFor(v => v.Id) ...
      }
    }
    
    public class Handler : IRequestHandler<Command, int>
    {
      private readonly IEnsekDbContext _db;
      private readonly IMapper _mapper;

      public Handler(IEnsekDbContext db, IMapper mapper)
      {
        _db = db;
        _mapper = mapper;
      }

      public async Task<int> Handle(Command command, CancellationToken token)
      {
        var entity = new MeterReading
        {
          RecordedAt = command.RecordedAt,
          AccountId = command.AccountId,
          Value = command.Value,
        };

        await _db.MeterReadings.AddAsync(entity, token);
        await _db.SaveChangesAsync(token);

        return entity.Id;

      }
      
    }
    
  }
  
}