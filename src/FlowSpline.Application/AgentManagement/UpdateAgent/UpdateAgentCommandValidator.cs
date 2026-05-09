using FluentValidation;

namespace FlowSpline.Application.AgentManagement.UpdateAgent;

public sealed class UpdateAgentCommandValidator : AbstractValidator<UpdateAgentCommand>
{
    public UpdateAgentCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        When(x => x.SystemPrompt != null, () =>
            RuleFor(x => x.SystemPrompt!).NotEmpty());
    }
}
