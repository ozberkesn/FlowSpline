using FlowSpline.Application.ToolRuntime.DTOs;
using FlowSpline.Application.ToolRuntime.Repositories;
using FlowSpline.Domain.ToolRuntime.Aggregates;
using MediatR;

namespace FlowSpline.Application.ToolRuntime.GetTools;

public sealed class GetToolsQueryHandler : IRequestHandler<GetToolsQuery, IReadOnlyList<ToolDefinitionDto>>
{
    private readonly IToolDefinitionRepository _repository;

    public GetToolsQueryHandler(IToolDefinitionRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<ToolDefinitionDto>> Handle(GetToolsQuery request, CancellationToken cancellationToken)
    {
        var tools = await _repository.GetAllAsync(cancellationToken);
        return tools.Select(ToDto).ToList();
    }

    internal static ToolDefinitionDto ToDto(ToolDefinition tool) =>
        new(tool.Id,
            tool.Name,
            tool.Description,
            tool.IsEnabled,
            tool.Schema.InputSchema,
            tool.Schema.OutputSchema);
}
