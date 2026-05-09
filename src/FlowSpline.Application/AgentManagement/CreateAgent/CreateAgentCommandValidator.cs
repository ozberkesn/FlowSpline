using FluentValidation;

namespace FlowSpline.Application.AgentManagement.CreateAgent;

public sealed class CreateAgentCommandValidator : AbstractValidator<CreateAgentCommand>
{
    public CreateAgentCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MinimumLength(3).MaximumLength(200);
        RuleFor(x => x.SystemPrompt).NotEmpty();
        RuleFor(x => x.Provider).NotEmpty();
        RuleFor(x => x.Model).NotEmpty();
        RuleFor(x => x.Temperature).InclusiveBetween(0.0, 2.0);
        RuleFor(x => x.MaxTokens).GreaterThan(0);
    }
}
