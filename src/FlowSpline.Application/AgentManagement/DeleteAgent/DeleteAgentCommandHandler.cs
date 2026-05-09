using FlowSpline.Application.AgentManagement.Repositories;
using MediatR;

namespace FlowSpline.Application.AgentManagement.DeleteAgent;

public sealed class DeleteAgentCommandHandler : IRequestHandler<DeleteAgentCommand>
{
    private readonly IAgentRepository _repository;

    public DeleteAgentCommandHandler(IAgentRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteAgentCommand request, CancellationToken cancellationToken)
    {
        if (!await _repository.ExistsAsync(request.Id, cancellationToken))
            throw new KeyNotFoundException($"Agent {request.Id} not found.");

        await _repository.DeleteAsync(request.Id, cancellationToken);
    }
}
