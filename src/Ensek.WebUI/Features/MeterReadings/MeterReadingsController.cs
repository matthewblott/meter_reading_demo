namespace Ensek.WebUI.Features.MeterReadings
{
  using System;
  using System.Threading.Tasks;
  using Application.MeterReadings.Commands;
  using MediatR;
  using Microsoft.AspNetCore.Mvc;
  using Application.MeterReadings.Queries;
  using Microsoft.AspNetCore.Authorization;
  using Index = Application.MeterReadings.Queries.Index;

  public class MeterReadingsController : Controller
  {
    private readonly IMediator _mediator;

    public MeterReadingsController(IMediator mediator) => _mediator = mediator;

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
    
    public IActionResult Import() => View();

    [HttpPost]
    public async Task<IActionResult> Import(Import.Command command) => View(await _mediator.Send(command));
    
  }
  
}