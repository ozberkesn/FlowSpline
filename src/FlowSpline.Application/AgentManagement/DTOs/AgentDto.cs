namespace FlowSpline.Application.AgentManagement.DTOs;

public sealed record AgentDto(
    Guid Id,
    string Name,
    string SystemPrompt,
    bool IsActive,
    string Provider,
    string Model,
    double Temperature,
    int MaxTokens,
    IReadOnlyList<string> ToolNames);
