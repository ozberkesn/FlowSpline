using FlowSpline.Application.AgentManagement.Repositories;
using MediatR;

namespace FlowSpline.Application.AgentManagement.UpdateAgent;

public sealed class UpdateAgentCommandHandler : IRequestHandler<UpdateAgentCommand>
{
    private readonly IAgentRepository _repository;

    public UpdateAgentCommandHandler(IAgentRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(UpdateAgentCommand request, CancellationToken cancellationToken)
    {
        var agent = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Agent {request.Id} not found.");

        if (request.SystemPrompt is not null)
            agent.ChangePrompt(request.SystemPrompt);

        if (request.IsActive is true)
            agent.Activate();
        else if (request.IsActive is false)
            agent.Deactivate();

        await _repository.UpdateAsync(agent, cancellationToken);
    }
}
