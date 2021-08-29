namespace Ensek.WebUI.Features.Accounts
{
  using Microsoft.AspNetCore.Mvc;
  using Application.Accounts.Queries;
  using System.Threading.Tasks;
  using Application.Accounts.Commands;
  using MediatR;
  
  public class AccountsController : Controller
  {
    private readonly IMediator _mediator;

    public AccountsController(IMediator mediator) => _mediator = mediator;

    public async Task<IActionResult> Index(Index.Query query) => View(await _mediator.Send(query));
    public async Task<IActionResult> Details(Details.Query query) => View(await _mediator.Send(query));

    public IActionResult New() => View();
    
    [HttpPost]
    public async Task<IActionResult> Create(Create.Command command)
    {
      await _mediator.Send(command);

      return RedirectToAction("Index");
    }
    
    [HttpPost]
    public async Task<IActionResult> Update(Update.Command command)
    {
      await _mediator.Send(command);

      return NoContent();
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Delete.Command command)
    {
      await _mediator.Send(command);

      return RedirectToAction("Index");

    }
    
  }
  
}