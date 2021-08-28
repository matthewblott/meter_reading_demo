namespace Ensek.Application.MeterReadings.Commands
{
  using Microsoft.EntityFrameworkCore;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading;
  using System.Threading.Tasks;
  using AutoMapper;
  using Common.Interfaces;
  using Domain.Entities;
  using FluentValidation;
  using MediatR;
  using Microsoft.AspNetCore.Http;

  public class Import
  {
    public class Command : IRequest<IEnumerable<Model>>
    {
      public IFormFile File { get; set; }
    }

    public class Model
    {
      public int RowNumber { get; set; }
      public int Id { get; set; }
      public int AccountId { get; set; }
      public string RecordedAt { get; set; }
      public int Value { get; set; }
      public bool IsValid { get; set; }
    }
    
    public class ModelValidator : AbstractValidator<Model>
    {
      private IEnsekDbContext _db;
      
      public ModelValidator(IEnsekDbContext db)
      {
        _db = db;
        
        RuleFor(v => v.Value).GreaterThanOrEqualTo(m => 0);
        RuleFor(v => v.Value).LessThanOrEqualTo(m => 99999);
        RuleFor(v => v.AccountId).MustAsync(AccountIdExists);
        //RuleFor(v => v).Must(IsNotDuplicate);
    
      }
      private async Task<bool> AccountIdExists(int value, CancellationToken token) =>
        await _db.Accounts.AnyAsync(a => a.Id == value, token);
    
      // private bool IsNotDuplicate(Model model)
      // {
      //   if(_values.Count(v => v.AccountId == model.AccountId) <= 1)
      //   {
      //     return true;
      //   }
      //
      //   // If it's the first appearance then it's a valid record
      //   return _values.First(v => v.AccountId == model.AccountId).RowNumber == model.RowNumber;
      // }
      
    }

    public class Validator : AbstractValidator<Command>
    {
      public Validator()
      {
        RuleFor(v => v.File.Length > 0);
      }
    }
    
    public class Handler : IRequestHandler<Command, IEnumerable<Model>>
    {
      private readonly IEnsekDbContext _db;
      private readonly IMapper _mapper;
      private readonly ICsvFileReader _fileReader;

      public Handler(IEnsekDbContext db, IMapper mapper, ICsvFileReader fileReader)
      {
        _db = db;
        _mapper = mapper;
        _fileReader = fileReader;
      }

      public async Task<IEnumerable<Model>> Handle(Command command, CancellationToken token)
      {
        var readings = _fileReader.ReadMeterReadingsFile(command.File).ToList();

        foreach (var reading in readings)
        {
          var validator = new ModelValidator(_db);
          var result = await validator.ValidateAsync(reading, token);
          
          //reading.IsValid = result.IsValid;
          
          if (!result.IsValid)
          {
            continue;
          }
          
          var entity = new MeterReading
          {
            RecordedAt = reading.RecordedAt,
            AccountId = reading.AccountId,
            Value = reading.Value,
          };

          await _db.MeterReadings.AddAsync(entity, token);
          await _db.SaveChangesAsync(token);

          reading.Id = entity.Id;

        }

        return readings;

      }
      
    }
    
  }
  
}