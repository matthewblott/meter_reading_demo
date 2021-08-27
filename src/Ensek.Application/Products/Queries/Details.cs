namespace Ensek.Application.Products.Queries
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
    
    public class Model : IMapFrom<Product>
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
        profile.CreateMap<Product, Model>();
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
        var entity = await _db.Products.FindAsync(query.Id);

        return _mapper.Map<Model>(entity);
      }
      
    }
    
  }
  
}