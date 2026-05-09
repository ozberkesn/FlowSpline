using FlowSpline.Application.AgentManagement.DTOs;
using MediatR;

namespace FlowSpline.Application.AgentManagement.GetAgent;

public sealed record GetAgentQuery(Guid Id) : IRequest<AgentDto?>;
