using MediatR;

namespace FlowSpline.Application.AgentManagement.DeleteAgent;

public sealed record DeleteAgentCommand(Guid Id) : IRequest;
