namespace Ensek.Application.MeterReadings.Commands
{
  using System;
  using System.Data;
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
    public class Command : IRequest<Model>
    {
      public IFormFile File { get; set; }
    }

    public class Model
    {
      public int Imported { get; set; }
      public int Failed { get; set; }
      
      public class CsvRow
      {
        public int RowNumber { get; set; }
        public int Id { get; set; }
        public int AccountId { get; set; }
        public DateTime RecordedAt { get; set; }
        public int Value { get; set; }
        public bool IsValid { get; set; }
      }

    }
    
    public class ModelValidator : AbstractValidator<Model.CsvRow>
    {
      private IEnsekDbContext _db;
      
      public ModelValidator(IEnsekDbContext db)
      {
        _db = db;
        
        RuleFor(v => v.Value).GreaterThanOrEqualTo(m => 0);
        RuleFor(v => v.Value).LessThanOrEqualTo(m => 99999);
        RuleFor(v => v.AccountId).MustAsync(AccountIdExists);
      }
      private async Task<bool> AccountIdExists(int value, CancellationToken token) =>
        await _db.Accounts.AnyAsync(a => a.Id == value, token);

    }

    public class Validator : AbstractValidator<Command>
    {
      public Validator()
      {
        RuleFor(v => v.File.Length > 0);
      }
    }
    
    public class Handler : IRequestHandler<Command, Model>
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

      private bool IsNotDuplicate(IList<Model.CsvRow> values, Model.CsvRow csvRow)
      {
        if(values.Count(v => v.AccountId == csvRow.AccountId) <= 1)
        {
          return true;
        }
      
        // If it's the first appearance then it's a valid record
        return values.First(v => v.AccountId == csvRow.AccountId).RowNumber == csvRow.RowNumber;
        
      }
      
      public async Task<Model> Handle(Command command, CancellationToken token)
      {
        var readings = _fileReader.ReadMeterReadingsFile(command.File).ToList();

        readings.ForEach(async r =>
        {
          var validator = new ModelValidator(_db);
          var result = await validator.ValidateAsync(r, token);

          if (!result.IsValid)
          {
            r.IsValid = false;
          }
          
        });

        var validSchemaReadings = readings.Where(r => r.IsValid);

        readings.ForEach(async r =>
        {
          if (!IsNotDuplicate(validSchemaReadings.ToList(), r))
          {
            r.IsValid = false;
          }          
        });

        var validReadings = readings.Where(r => r.IsValid);
        
        foreach (var reading in validReadings)
        {
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

        var totalNumberOfRows = readings.Count;
        var importedNumber = validReadings.Count();
        var failedNumber = totalNumberOfRows - importedNumber;
        
        var model = new Model
        {
          Imported = importedNumber,
          Failed = failedNumber,
        };

        return model;

      }
      
    }
    
  }
  
}