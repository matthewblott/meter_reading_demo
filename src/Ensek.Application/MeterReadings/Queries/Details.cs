using System;

namespace Ensek.Application.MeterReadings.Queries
{
  using AutoMapper;
  using Domain.Entities;
  using System.Threading;
  using System.Threading.Tasks;
  using Common.Interfaces;
  using Common.Mappings;
  using MediatR;

  public class Details
  {
    public class Query : IRequest<Model>
    {
      public int Id { get; set; }
    }
    
    public class Model : IMapFrom<MeterReading>
    {
      public int Id { get; set; }
      public DateTime RecordedAt { get; set; }
      public int AccountId { get; set; }
      public int Value { get; set; }

      public void Mapping(Profile profile)
      {
        profile.CreateMap<MeterReading, Model>();
      }

    }

    public class Handler : IRequestHandler<Query, Model>
    {
      private readonly IEnsekDbContext _db;
      private readonly IMapper _mapper;

      public Handler(IEnsekDbContext db, IMapper mapper)
      {
        _db = db;
        _mapper = mapper;
      }

      public async Task<Model> Handle(Query query, CancellationToken token)
      {
        var entity = await _db.MeterReadings.FindAsync(query.Id);

        return _mapper.Map<Model>(entity);
      }
      
    }
    
  }
  
}