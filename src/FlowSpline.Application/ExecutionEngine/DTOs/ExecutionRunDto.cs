namespace FlowSpline.Application.ExecutionEngine.DTOs;

public sealed record ExecutionRunDto(
    Guid Id,
    Guid AgentId,
    string Input,
    Guid SessionId,
    string Status,
    DateTimeOffset? StartedAt,
    DateTimeOffset? CompletedAt,
    string? FailureReason,
    int RetryCount);
