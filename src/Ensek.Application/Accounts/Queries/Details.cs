namespace Ensek.Application.Accounts.Queries
{
  using System;
  using System.ComponentModel.DataAnnotations;
  using AutoMapper;
  using Domain.Entities;
  using System.Threading;
  using System.Threading.Tasks;
  using Common.Interfaces;
  using Common.Mappings;
  using FluentValidation;
  using MediatR;

  public class Details
  {
    public class Query : IRequest<Model>
    {
      public int Id { get; set; }
    }
    
    public class Validator : AbstractValidator<Query>
    {
      public Validator()
      {
      }
    }

    public class Model : IMapFrom<Account>
    {
      public int Id { get; set; }
      public string FirstName { get; set; }
      [Required]
      public string LastName { get; set; }

      public void Mapping(Profile profile)
      {
        profile.CreateMap<Account, Model>()
          .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id));
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
        var id = query.Id;
        var entity = await _db.Accounts.FindAsync(id);

        return _mapper.Map<Model>(entity);
      }
      
    }
    
  }
  
}