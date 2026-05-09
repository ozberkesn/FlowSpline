using FlowSpline.Application.AgentManagement.DTOs;
using FlowSpline.Application.AgentManagement.GetAgent;
using FlowSpline.Application.AgentManagement.Repositories;
using MediatR;

namespace FlowSpline.Application.AgentManagement.GetAgents;

public sealed class GetAgentsQueryHandler : IRequestHandler<GetAgentsQuery, IReadOnlyList<AgentDto>>
{
    private readonly IAgentRepository _repository;

    public GetAgentsQueryHandler(IAgentRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<AgentDto>> Handle(GetAgentsQuery request, CancellationToken cancellationToken)
    {
        var agents = await _repository.GetAllAsync(cancellationToken);
        return agents.Select(GetAgentQueryHandler.ToDto).ToList();
    }
}
