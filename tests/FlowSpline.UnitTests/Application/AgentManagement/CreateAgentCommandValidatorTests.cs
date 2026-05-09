using FlowSpline.Application.AgentManagement.CreateAgent;

namespace FlowSpline.UnitTests.Application.AgentManagement;

public class CreateAgentCommandValidatorTests
{
    private readonly CreateAgentCommandValidator _validator = new();

    [Fact]
    public void Validate_WhenNameTooShort_ShouldHaveError()
    {
        var result = _validator.Validate(ValidCommand() with { Name = "ab" });

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateAgentCommand.Name));
    }

    [Fact]
    public void Validate_WhenTemperatureAboveTwo_ShouldHaveError()
    {
        var result = _validator.Validate(ValidCommand() with { Temperature = 2.1 });

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateAgentCommand.Temperature));
    }

    [Fact]
    public void Validate_WhenMaxTokensZero_ShouldHaveError()
    {
        var result = _validator.Validate(ValidCommand() with { MaxTokens = 0 });

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateAgentCommand.MaxTokens));
    }

    [Fact]
    public void Validate_WhenAllValid_ShouldPassValidation()
    {
        var result = _validator.Validate(ValidCommand());

        Assert.True(result.IsValid);
    }

    private static CreateAgentCommand ValidCommand() =>
        new("Test Agent", "You are helpful.", "OpenAI", "gpt-4o", 0.7, 1000);
}
