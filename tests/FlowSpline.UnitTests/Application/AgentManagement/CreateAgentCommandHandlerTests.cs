using FlowSpline.Application.AgentManagement.CreateAgent;
using FlowSpline.Application.AgentManagement.Repositories;
using FlowSpline.Domain.AgentManagement.Aggregates;
using Moq;

namespace FlowSpline.UnitTests.Application.AgentManagement;

public class CreateAgentCommandHandlerTests
{
    private readonly Mock<IAgentRepository> _repoMock = new();
    private readonly CreateAgentCommandHandler _handler;

    public CreateAgentCommandHandlerTests()
    {
        _handler = new CreateAgentCommandHandler(_repoMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldReturnNonEmptyGuid()
    {
        var command = ValidCommand();

        var id = await _handler.Handle(command, CancellationToken.None);

        Assert.NotEqual(Guid.Empty, id);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldCallAddAsyncOnce()
    {
        var command = ValidCommand();

        await _handler.Handle(command, CancellationToken.None);

        _repoMock.Verify(r => r.AddAsync(It.IsAny<AgentDefinition>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenDomainConstructorThrows_ShouldPropagateException()
    {
        var command = ValidCommand() with { Temperature = 5.0 };

        await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
    }

    private static CreateAgentCommand ValidCommand() =>
        new("Test Agent", "You are helpful.", "OpenAI", "gpt-4o", 0.7, 1000);
}
