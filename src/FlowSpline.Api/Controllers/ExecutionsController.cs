using FlowSpline.Application.ExecutionEngine.CreateExecution;
using FlowSpline.Application.ExecutionEngine.GetExecution;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FlowSpline.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExecutionsController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateExecutionCommand command, CancellationToken ct)
    {
        var id = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var execution = await mediator.Send(new GetExecutionQuery(id), ct);
        return execution is null ? NotFound() : Ok(execution);
    }
}
