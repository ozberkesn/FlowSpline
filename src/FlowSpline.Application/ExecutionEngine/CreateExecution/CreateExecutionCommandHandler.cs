using FlowSpline.Application.AgentManagement.Repositories;
using FlowSpline.Application.ExecutionEngine.Repositories;
using FlowSpline.Domain.ExecutionEngine.Aggregates;
using FlowSpline.Domain.ExecutionEngine.ValueObjects;
using MediatR;

namespace FlowSpline.Application.ExecutionEngine.CreateExecution;

public sealed class CreateExecutionCommandHandler : IRequestHandler<CreateExecutionCommand, Guid>
{
    private readonly IExecutionRunRepository _executionRepository;
    private readonly IAgentRepository _agentRepository;

    public CreateExecutionCommandHandler(
        IExecutionRunRepository executionRepository,
        IAgentRepository agentRepository)
    {
        _executionRepository = executionRepository;
        _agentRepository = agentRepository;
    }

    public async Task<Guid> Handle(CreateExecutionCommand request, CancellationToken cancellationToken)
    {
        var agent = await _agentRepository.GetByIdAsync(request.AgentId, cancellationToken)
            ?? throw new KeyNotFoundException($"Agent {request.AgentId} not found.");

        if (!agent.IsActive)
            throw new InvalidOperationException($"Agent {request.AgentId} is not active.");

        var context = new RunContext(request.AgentId, request.Input, request.SessionId);
        var run = new ExecutionRun(context);

        await _executionRepository.AddAsync(run, cancellationToken);

        return run.Id;
    }
}
