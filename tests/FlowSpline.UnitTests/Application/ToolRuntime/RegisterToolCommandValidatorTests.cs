using FlowSpline.Application.ToolRuntime.RegisterTool;

namespace FlowSpline.UnitTests.Application.ToolRuntime;

public class RegisterToolCommandValidatorTests
{
    private readonly RegisterToolCommandValidator _validator = new();

    [Fact]
    public void Validate_WhenNameHasUppercase_ShouldHaveError()
    {
        var result = _validator.Validate(ValidCommand() with { Name = "SearchTool" });

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(RegisterToolCommand.Name));
    }

    [Fact]
    public void Validate_WhenNameHasSpaces_ShouldHaveError()
    {
        var result = _validator.Validate(ValidCommand() with { Name = "search tool" });

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(RegisterToolCommand.Name));
    }

    [Fact]
    public void Validate_WhenNameIsValidSlug_ShouldPassValidation()
    {
        var result = _validator.Validate(ValidCommand() with { Name = "web-search_v2" });

        Assert.True(result.IsValid);
    }

    private static RegisterToolCommand ValidCommand() =>
        new("search", "A web search tool", null, null);
}
