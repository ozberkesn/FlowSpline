using MediatR;

namespace FlowSpline.Application.AgentManagement.UpdateAgent;

public sealed record UpdateAgentCommand(
    Guid Id,
    string? SystemPrompt,
    bool? IsActive) : IRequest;
