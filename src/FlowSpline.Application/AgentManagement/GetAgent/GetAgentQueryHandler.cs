using FlowSpline.Application.AgentManagement.DTOs;
using FlowSpline.Application.AgentManagement.Repositories;
using FlowSpline.Domain.AgentManagement.Aggregates;
using MediatR;

namespace FlowSpline.Application.AgentManagement.GetAgent;

public sealed class GetAgentQueryHandler : IRequestHandler<GetAgentQuery, AgentDto?>
{
    private readonly IAgentRepository _repository;

    public GetAgentQueryHandler(IAgentRepository repository)
    {
        _repository = repository;
    }

    public async Task<AgentDto?> Handle(GetAgentQuery request, CancellationToken cancellationToken)
    {
        var agent = await _repository.GetByIdAsync(request.Id, cancellationToken);
        return agent is null ? null : ToDto(agent);
    }

    internal static AgentDto ToDto(AgentDefinition agent) =>
        new(agent.Id,
            agent.Name,
            agent.SystemPrompt,
            agent.IsActive,
            agent.Model.Provider,
            agent.Model.Model,
            agent.Model.Temperature,
            agent.Model.MaxTokens,
            agent.Tools.Select(t => t.Name).ToList());
}
