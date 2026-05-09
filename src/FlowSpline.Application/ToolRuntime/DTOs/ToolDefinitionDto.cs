namespace FlowSpline.Application.ToolRuntime.DTOs;

public sealed record ToolDefinitionDto(
    Guid Id,
    string Name,
    string Description,
    bool IsEnabled,
    string? InputSchema,
    string? OutputSchema);
