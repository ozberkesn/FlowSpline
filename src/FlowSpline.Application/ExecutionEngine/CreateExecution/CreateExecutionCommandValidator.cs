using FluentValidation;

namespace FlowSpline.Application.ExecutionEngine.CreateExecution;

public sealed class CreateExecutionCommandValidator : AbstractValidator<CreateExecutionCommand>
{
    public CreateExecutionCommandValidator()
    {
        RuleFor(x => x.AgentId).NotEmpty();
        RuleFor(x => x.Input).NotEmpty();
        RuleFor(x => x.SessionId).NotEmpty();
    }
}
