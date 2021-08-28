namespace Ensek.Application.Accounts.Commands
{
  using System;
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
      public string FirstName { get; set; }
      public string LastName { get; set; }
    }

    public class Validator : AbstractValidator<Command>
    {
      public Validator()
      {
        RuleFor(v => v.FirstName).MaximumLength(50).NotEmpty();
        RuleFor(v => v.LastName).MaximumLength(50).NotEmpty();
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
        var entity = new Account
        {
          FirstName = command.FirstName,
          LastName = command.LastName,
        };

        await _db.Accounts.AddAsync(entity, token);
        await _db.SaveChangesAsync(token);

        return entity.Id;

      }
      
    }
    
  }
  
}