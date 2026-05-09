using FlowSpline.Application.AgentManagement.Repositories;
using FlowSpline.Domain.AgentManagement.Aggregates;
using FlowSpline.Domain.AgentManagement.ValueObjects;
using MediatR;

namespace FlowSpline.Application.AgentManagement.CreateAgent;

public sealed class CreateAgentCommandHandler : IRequestHandler<CreateAgentCommand, Guid>
{
    private readonly IAgentRepository _repository;

    public CreateAgentCommandHandler(IAgentRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(CreateAgentCommand request, CancellationToken cancellationToken)
    {
        var model = new ModelSettings(request.Provider, request.Model, request.Temperature, request.MaxTokens);
        var agent = new AgentDefinition(request.Name, request.SystemPrompt, model);

        await _repository.AddAsync(agent, cancellationToken);

        return agent.Id;
    }
}
