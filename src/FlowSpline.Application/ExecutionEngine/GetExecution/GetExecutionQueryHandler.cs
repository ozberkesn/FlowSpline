using FlowSpline.Application.ExecutionEngine.DTOs;
using FlowSpline.Application.ExecutionEngine.Repositories;
using FlowSpline.Domain.ExecutionEngine.Aggregates;
using MediatR;

namespace FlowSpline.Application.ExecutionEngine.GetExecution;

public sealed class GetExecutionQueryHandler : IRequestHandler<GetExecutionQuery, ExecutionRunDto?>
{
    private readonly IExecutionRunRepository _repository;

    public GetExecutionQueryHandler(IExecutionRunRepository repository)
    {
        _repository = repository;
    }

    public async Task<ExecutionRunDto?> Handle(GetExecutionQuery request, CancellationToken cancellationToken)
    {
        var run = await _repository.GetByIdAsync(request.Id, cancellationToken);
        return run is null ? null : ToDto(run);
    }

    internal static ExecutionRunDto ToDto(ExecutionRun run) =>
        new(run.Id,
            run.Context.AgentId,
            run.Context.Input,
            run.Context.SessionId,
            run.Status.ToString(),
            run.StartedAt,
            run.CompletedAt,
            run.FailureReason,
            run.RetryCount);
}
