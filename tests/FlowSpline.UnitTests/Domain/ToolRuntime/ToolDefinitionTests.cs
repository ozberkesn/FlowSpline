using FlowSpline.Domain.ToolRuntime.Aggregates;
using FlowSpline.Domain.ToolRuntime.ValueObjects;

namespace FlowSpline.UnitTests.Domain.ToolRuntime;

public class ToolDefinitionTests
{
    [Fact]
    public void CreateTool_WithValidData_ShouldBeEnabled()
    {
        var tool = CreateTool();

        Assert.NotEqual(Guid.Empty, tool.Id);
        Assert.Equal("web-search", tool.Name);
        Assert.True(tool.IsEnabled);
    }

    [Fact]
    public void CreateTool_WithInvalidSlugName_ShouldThrow()
    {
        var schema = new ToolSchema(null, null);

        Assert.Throws<ArgumentException>(() =>
            new ToolDefinition("Web Search", "Searches the web", schema));
    }

    [Fact]
    public void CreateTool_WithUppercaseName_ShouldThrow()
    {
        var schema = new ToolSchema(null, null);

        Assert.Throws<ArgumentException>(() =>
            new ToolDefinition("WebSearch", "Searches the web", schema));
    }

    [Fact]
    public void Disable_ShouldMakeToolDisabled()
    {
        var tool = CreateTool();

        tool.Disable();

        Assert.False(tool.IsEnabled);
    }

    [Fact]
    public void Enable_ShouldMakeToolEnabled()
    {
        var tool = CreateTool();
        tool.Disable();

        tool.Enable();

        Assert.True(tool.IsEnabled);
    }

    [Fact]
    public void Enable_WhenAlreadyEnabled_ShouldNotRaiseEvent()
    {
        var tool = CreateTool();
        tool.ClearDomainEvents();

        tool.Enable();

        Assert.Empty(tool.DomainEvents);
    }

    [Fact]
    public void Disable_WhenAlreadyDisabled_ShouldNotRaiseEvent()
    {
        var tool = CreateTool();
        tool.Disable();
        tool.ClearDomainEvents();

        tool.Disable();

        Assert.Empty(tool.DomainEvents);
    }

    [Fact]
    public void UpdateDescription_WithValidText_ShouldUpdateAndRaiseEvent()
    {
        var tool = CreateTool();
        tool.ClearDomainEvents();

        tool.UpdateDescription("Updated description");

        Assert.Equal("Updated description", tool.Description);
        Assert.Single(tool.DomainEvents);
    }

    [Fact]
    public void UpdateDescription_WithEmpty_ShouldThrow()
    {
        var tool = CreateTool();

        Assert.Throws<ArgumentException>(() => tool.UpdateDescription(""));
    }

    [Fact]
    public void UpdateSchema_ShouldUpdateAndRaiseEvent()
    {
        var tool = CreateTool();
        tool.ClearDomainEvents();
        var newSchema = new ToolSchema("{}", "{}");

        tool.UpdateSchema(newSchema);

        Assert.Equal("{}", tool.Schema.InputSchema);
        Assert.Single(tool.DomainEvents);
    }

    [Fact]
    public void CreateTool_ShouldRaiseDomainEvent()
    {
        var tool = CreateTool();

        Assert.Single(tool.DomainEvents);
    }

    private static ToolDefinition CreateTool() =>
        new("web-search", "Searches the web", new ToolSchema(null, null));
}
