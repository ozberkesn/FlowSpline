using FlowSpline.Application.ToolRuntime.GetTools;
using FlowSpline.Application.ToolRuntime.RegisterTool;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FlowSpline.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ToolsController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterToolCommand command, CancellationToken ct)
    {
        var id = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetAll), new { id }, new { id });
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var tools = await mediator.Send(new GetToolsQuery(), ct);
        return Ok(tools);
    }
}
