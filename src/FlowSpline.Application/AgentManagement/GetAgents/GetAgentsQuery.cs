using FlowSpline.Application.AgentManagement.DTOs;
using MediatR;

namespace FlowSpline.Application.AgentManagement.GetAgents;

public sealed record GetAgentsQuery : IRequest<IReadOnlyList<AgentDto>>;
