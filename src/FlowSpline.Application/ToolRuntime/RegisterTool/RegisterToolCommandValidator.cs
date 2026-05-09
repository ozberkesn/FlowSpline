using FluentValidation;

namespace FlowSpline.Application.ToolRuntime.RegisterTool;

public sealed class RegisterToolCommandValidator : AbstractValidator<RegisterToolCommand>
{
    public RegisterToolCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .Matches(@"^[a-z0-9_-]+$")
            .WithMessage("Tool name must be lowercase alphanumeric with hyphens or underscores only.");
        RuleFor(x => x.Description).NotEmpty();
    }
}
