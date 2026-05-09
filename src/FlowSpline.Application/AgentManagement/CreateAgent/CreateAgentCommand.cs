using MediatR;

namespace FlowSpline.Application.AgentManagement.CreateAgent;

public sealed record CreateAgentCommand(
    string Name,
    string SystemPrompt,
    string Provider,
    string Model,
    double Temperature,
    int MaxTokens) : IRequest<Guid>;
