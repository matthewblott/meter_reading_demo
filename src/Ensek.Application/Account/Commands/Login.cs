namespace Ensek.Application.Account.Commands
{
  using System;
  using System.Threading;
  using System.Threading.Tasks;
  using FluentValidation;
  using MediatR;
  using System.Collections.Generic;
  using System.Security.Claims;
  using System.Security.Principal;
  using Common.Interfaces;
  using Microsoft.AspNetCore.Authentication.Cookies;

  public class Login
  {
    public class Command : IRequest<IPrincipal>
    {
      public string Username { get; set; }
      public string Password { get; set; }
    }
  
    public class Validator : AbstractValidator<Command>
    {
      
    }

    public class Handler : IRequestHandler<Command, IPrincipal>
    {
      private readonly IEnsekDbContext _db;

      public Handler(IEnsekDbContext db) => _db = db;

      public Task<IPrincipal> Handle(Command command, CancellationToken cancellationToken)
      {
        if (command.Username != "admin" || command.Password != "password")
        {
          throw new Exception();
        }
        
        var claims = new List<Claim>
        {
          new Claim(ClaimTypes.NameIdentifier, "admin"), 
          new Claim(ClaimTypes.Name, "admin"),
          new Claim(ClaimTypes.Role, "Admins"),
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        
        IPrincipal principal = new ClaimsPrincipal(identity);

        return Task.FromResult(principal);

      }
      
    }
    
  }
  
}