using FlowSpline.Application.ToolRuntime.Repositories;
using FlowSpline.Domain.ToolRuntime.Aggregates;
using FlowSpline.Domain.ToolRuntime.ValueObjects;
using MediatR;

namespace FlowSpline.Application.ToolRuntime.RegisterTool;

public sealed class RegisterToolCommandHandler : IRequestHandler<RegisterToolCommand, Guid>
{
    private readonly IToolDefinitionRepository _repository;

    public RegisterToolCommandHandler(IToolDefinitionRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(RegisterToolCommand request, CancellationToken cancellationToken)
    {
        if (await _repository.ExistsByNameAsync(request.Name, cancellationToken))
            throw new InvalidOperationException($"Tool name '{request.Name}' is already registered.");

        var schema = new ToolSchema(request.InputSchema, request.OutputSchema);
        var tool = new ToolDefinition(request.Name, request.Description, schema);

        await _repository.AddAsync(tool, cancellationToken);

        return tool.Id;
    }
}
