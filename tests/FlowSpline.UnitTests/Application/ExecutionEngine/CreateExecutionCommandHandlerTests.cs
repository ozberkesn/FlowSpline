using FlowSpline.Application.AgentManagement.Repositories;
using FlowSpline.Application.ExecutionEngine.CreateExecution;
using FlowSpline.Application.ExecutionEngine.Repositories;
using FlowSpline.Domain.AgentManagement.Aggregates;
using FlowSpline.Domain.AgentManagement.ValueObjects;
using FlowSpline.Domain.ExecutionEngine.Aggregates;
using Moq;

namespace FlowSpline.UnitTests.Application.ExecutionEngine;

public class CreateExecutionCommandHandlerTests
{
    private readonly Mock<IExecutionRunRepository> _executionRepoMock = new();
    private readonly Mock<IAgentRepository> _agentRepoMock = new();
    private readonly CreateExecutionCommandHandler _handler;

    public CreateExecutionCommandHandlerTests()
    {
        _handler = new CreateExecutionCommandHandler(_executionRepoMock.Object, _agentRepoMock.Object);
    }

    [Fact]
    public async Task Handle_WhenAgentNotFound_ShouldThrowKeyNotFoundException()
    {
        _agentRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((AgentDefinition?)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _handler.Handle(ValidCommand(), CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WhenAgentIsInactive_ShouldThrowInvalidOperationException()
    {
        var agent = CreateAgent();
        agent.Deactivate();
        _agentRepoMock.Setup(r => r.GetByIdAsync(agent.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(agent);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _handler.Handle(ValidCommand(agent.Id), CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldReturnNonEmptyGuid()
    {
        var agent = CreateAgent();
        _agentRepoMock.Setup(r => r.GetByIdAsync(agent.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(agent);

        var id = await _handler.Handle(ValidCommand(agent.Id), CancellationToken.None);

        Assert.NotEqual(Guid.Empty, id);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldNotCallStartOnRun()
    {
        var agent = CreateAgent();
        _agentRepoMock.Setup(r => r.GetByIdAsync(agent.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(agent);

        await _handler.Handle(ValidCommand(agent.Id), CancellationToken.None);

        _executionRepoMock.Verify(
            r => r.AddAsync(It.Is<ExecutionRun>(run => run.StartedAt == null), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    private static AgentDefinition CreateAgent() =>
        new("Test Agent", "You are helpful.", new ModelSettings("OpenAI", "gpt-4o", 0.7, 1000));

    private static CreateExecutionCommand ValidCommand(Guid? agentId = null) =>
        new(agentId ?? Guid.NewGuid(), "What is 2+2?", Guid.NewGuid());
}
