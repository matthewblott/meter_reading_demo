namespace Ensek.Application.Products.Queries
{
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
      
      public class Item : IMapFrom<Product>
      {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public decimal? UnitPrice { get; set; }
        public int? SupplierId { get; set; }
        public string SupplierCompanyName { get; set; }
        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public bool Discontinued { get; set; }

        public void Mapping(Profile profile)
        {
          profile.CreateMap<Product, Item>()
            .ForMember(d => d.Id, opt => opt.MapFrom(s => s.ProductId));
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
        var items = await _db.Products
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