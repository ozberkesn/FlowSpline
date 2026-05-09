using MediatR;

namespace FlowSpline.Application.ToolRuntime.RegisterTool;

public sealed record RegisterToolCommand(
    string Name,
    string Description,
    string? InputSchema,
    string? OutputSchema) : IRequest<Guid>;
