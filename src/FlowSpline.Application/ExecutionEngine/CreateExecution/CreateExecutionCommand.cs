using MediatR;

namespace FlowSpline.Application.ExecutionEngine.CreateExecution;

public sealed record CreateExecutionCommand(
    Guid AgentId,
    string Input,
    Guid SessionId) : IRequest<Guid>;
