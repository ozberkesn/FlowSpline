using FlowSpline.Application.ToolRuntime.DTOs;
using MediatR;

namespace FlowSpline.Application.ToolRuntime.GetTools;

public sealed record GetToolsQuery : IRequest<IReadOnlyList<ToolDefinitionDto>>;
