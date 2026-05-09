using FlowSpline.Application.AgentManagement.Repositories;
using FlowSpline.Application.AgentManagement.UpdateAgent;
using FlowSpline.Domain.AgentManagement.Aggregates;
using FlowSpline.Domain.AgentManagement.ValueObjects;
using Moq;

namespace FlowSpline.UnitTests.Application.AgentManagement;

public class UpdateAgentCommandHandlerTests
{
    private readonly Mock<IAgentRepository> _repoMock = new();
    private readonly UpdateAgentCommandHandler _handler;

    public UpdateAgentCommandHandlerTests()
    {
        _handler = new UpdateAgentCommandHandler(_repoMock.Object);
    }

    [Fact]
    public async Task Handle_WhenAgentNotFound_ShouldThrowKeyNotFoundException()
    {
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((AgentDefinition?)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _handler.Handle(new UpdateAgentCommand(Guid.NewGuid(), null, null), CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WithNewPrompt_ShouldUpdateSystemPrompt()
    {
        var agent = CreateAgent();
        _repoMock.Setup(r => r.GetByIdAsync(agent.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(agent);

        await _handler.Handle(new UpdateAgentCommand(agent.Id, "New prompt", null), CancellationToken.None);

        Assert.Equal("New prompt", agent.SystemPrompt);
    }

    [Fact]
    public async Task Handle_WithIsActiveFalse_ShouldDeactivateAgent()
    {
        var agent = CreateAgent();
        _repoMock.Setup(r => r.GetByIdAsync(agent.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(agent);

        await _handler.Handle(new UpdateAgentCommand(agent.Id, null, false), CancellationToken.None);

        Assert.False(agent.IsActive);
    }

    [Fact]
    public async Task Handle_WithIsActiveTrue_ShouldActivateAgent()
    {
        var agent = CreateAgent();
        agent.Deactivate();
        _repoMock.Setup(r => r.GetByIdAsync(agent.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(agent);

        await _handler.Handle(new UpdateAgentCommand(agent.Id, null, true), CancellationToken.None);

        Assert.True(agent.IsActive);
    }

    private static AgentDefinition CreateAgent() =>
        new("Test Agent", "You are helpful.", new ModelSettings("OpenAI", "gpt-4o", 0.7, 1000));
}
