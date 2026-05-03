using FlowSpline.Domain.AgentManagement.Aggregates;
using FlowSpline.Domain.AgentManagement.ValueObjects;

namespace FlowSpline.UnitTests.Domain;

public class AgentDefinitionTests
{
    [Fact]
    public void CreateAgent_WithValidData_ShouldCreateActiveAgent()
    {
        var model = CreateModelSettings();

        var agent = new AgentDefinition(
            "Sales Agent",
            "You are a sales assistant.",
            model);

        Assert.NotEqual(Guid.Empty, agent.Id);
        Assert.Equal("Sales Agent", agent.Name);
        Assert.Equal("You are a sales assistant.", agent.SystemPrompt);
        Assert.Equal(model, agent.Model);
        Assert.True(agent.IsActive);
        Assert.Empty(agent.Tools);
    }

    [Fact]
    public void CreateAgent_ShouldTrimNameAndPrompt()
    {
        var model = CreateModelSettings();

        var agent = new AgentDefinition(
            "  Sales Agent  ",
            "  You are helpful.  ",
            model);

        Assert.Equal("Sales Agent", agent.Name);
        Assert.Equal("You are helpful.", agent.SystemPrompt);
    }

    [Fact]
    public void Deactivate_ShouldMakeAgentInactive()
    {
        var agent = CreateAgent();

        agent.Deactivate();

        Assert.False(agent.IsActive);
    }

    [Fact]
    public void Activate_ShouldMakeAgentActive()
    {
        var agent = CreateAgent();
        agent.Deactivate();

        agent.Activate();

        Assert.True(agent.IsActive);
    }

    private static AgentDefinition CreateAgent()
    {
        return new AgentDefinition(
            "Test Agent",
            "Test prompt",
            CreateModelSettings());
    }

    private static ModelSettings CreateModelSettings()
    {
        return new ModelSettings(
            "OpenAI",
            "gpt-4.1-mini",
            0.7,
            1000);
    }
}