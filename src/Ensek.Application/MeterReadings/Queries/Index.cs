namespace Ensek.Application.MeterReadings.Queries
{
  using System;
  using System.Threading;
  using System.Threading.Tasks;
  using AutoMapper;
  using AutoMapper.QueryableExtensions;
  using Common;
  using Common.Interfaces;
  using Common.Mappings;
  using Domain.Entities;
  using MediatR;
  using X.PagedList;

  public class Index
  {
    public class Query : IRequest<Model>
    {
      public int Page { get; set; } = 1;
    }

    public class Model
    {
      public IPagedList<Item> Items { get; set; }
      
      public bool CreateEnabled { get; set; }
      
      public class Item : IMapFrom<MeterReading>
      {
        public int Id { get; set; }
        public int? AccountId { get; set; }
        public DateTime RecordedAt { get; set; }
        public int Value { get; set; }

        public void Mapping(Profile profile)
        {
          profile.CreateMap<MeterReading, Item>()
            .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id));
        }
        
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
        var items = await _db.MeterReadings
          .ProjectTo<Model.Item>(_mapper.ConfigurationProvider)
          .ToPagedListAsync(query.Page, PageConstants.PageSize, token);

        var model = new Model
        {
          Items = items,
          CreateEnabled = true // TODO: Set based on user permissions.
        };
    
        return model;
        
      }
      
    }
    
  }

}