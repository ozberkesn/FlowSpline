using FlowSpline.Domain.Entities;

namespace FlowSpline.UnitTests.Domain;

public class AgentDefinitionTests
{
    [Fact]
    public void CreateAgent_WithValidData_ShouldSucceed()
    {
        var agent = new AgentDefinition(
            "Sales Agent",
            "You qualify leads",
            "gpt-4");

        Assert.Equal("Sales Agent", agent.Name);
        Assert.True(agent.IsActive);
    }

    [Fact]
    public void CreateAgent_WithoutName_ShouldThrow()
    {
        Assert.Throws<ArgumentException>(() =>
            new AgentDefinition(
                "",
                "Prompt",
                "gpt-4"));
    }

    [Fact]
    public void ChangePrompt_WithEmptyPrompt_ShouldThrow()
    {
        var agent = new AgentDefinition(
            "Agent",
            "Prompt",
            "gpt-4");

        Assert.Throws<ArgumentException>(() =>
            agent.ChangePrompt(""));
    }

    [Fact]
    public void Deactivate_ShouldSetInactive()
    {
        var agent = new AgentDefinition(
            "Agent",
            "Prompt",
            "gpt-4");

        agent.Deactivate();

        Assert.False(agent.IsActive);
    }

    [Fact]
    public void CreateAgent_WithShortName_ShouldThrow()
    {
        Assert.Throws<ArgumentException>(() =>
            new AgentDefinition(
                "A",
                "Prompt",
                "gpt-4"));
    }
}