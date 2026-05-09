using FlowSpline.Application.AgentManagement.CreateAgent;
using FlowSpline.Application.AgentManagement.DeleteAgent;
using FlowSpline.Application.AgentManagement.GetAgent;
using FlowSpline.Application.AgentManagement.GetAgents;
using FlowSpline.Application.AgentManagement.UpdateAgent;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FlowSpline.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AgentsController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAgentCommand command, CancellationToken ct)
    {
        var id = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var agents = await mediator.Send(new GetAgentsQuery(), ct);
        return Ok(agents);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var agent = await mediator.Send(new GetAgentQuery(id), ct);
        return agent is null ? NotFound() : Ok(agent);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAgentBody body, CancellationToken ct)
    {
        var agent = await mediator.Send(new GetAgentQuery(id), ct);
        if (agent is null) return NotFound();

        await mediator.Send(new UpdateAgentCommand(id, body.SystemPrompt, body.IsActive), ct);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await mediator.Send(new DeleteAgentCommand(id), ct);
        return NoContent();
    }

    public sealed record UpdateAgentBody(string? SystemPrompt, bool? IsActive);
}
