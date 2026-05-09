using FlowSpline.Application.AgentManagement.GetAgent;
using FlowSpline.Application.AgentManagement.Repositories;
using FlowSpline.Domain.AgentManagement.Aggregates;
using FlowSpline.Domain.AgentManagement.ValueObjects;
using Moq;

namespace FlowSpline.UnitTests.Application.AgentManagement;

public class GetAgentQueryHandlerTests
{
    private readonly Mock<IAgentRepository> _repoMock = new();
    private readonly GetAgentQueryHandler _handler;

    public GetAgentQueryHandlerTests()
    {
        _handler = new GetAgentQueryHandler(_repoMock.Object);
    }

    [Fact]
    public async Task Handle_WhenAgentExists_ShouldReturnAgentDto()
    {
        var agent = CreateAgent();
        _repoMock.Setup(r => r.GetByIdAsync(agent.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(agent);

        var result = await _handler.Handle(new GetAgentQuery(agent.Id), CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(agent.Id, result.Id);
        Assert.Equal(agent.Name, result.Name);
        Assert.Equal(agent.SystemPrompt, result.SystemPrompt);
        Assert.Equal(agent.IsActive, result.IsActive);
    }

    [Fact]
    public async Task Handle_WhenAgentNotFound_ShouldReturnNull()
    {
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((AgentDefinition?)null);

        var result = await _handler.Handle(new GetAgentQuery(Guid.NewGuid()), CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task Handle_WhenAgentHasTools_ShouldMapToolNamesCorrectly()
    {
        var agent = CreateAgent();
        agent.BindTool(new Tool("tool-a"));
        agent.BindTool(new Tool("tool-b"));
        _repoMock.Setup(r => r.GetByIdAsync(agent.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(agent);

        var result = await _handler.Handle(new GetAgentQuery(agent.Id), CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(2, result.ToolNames.Count);
        Assert.Contains("tool-a", result.ToolNames);
        Assert.Contains("tool-b", result.ToolNames);
    }

    private static AgentDefinition CreateAgent() =>
        new("Test Agent", "You are helpful.", new ModelSettings("OpenAI", "gpt-4o", 0.7, 1000));
}
