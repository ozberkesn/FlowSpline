using FlowSpline.Application.AgentManagement.DeleteAgent;
using FlowSpline.Application.AgentManagement.Repositories;
using Moq;

namespace FlowSpline.UnitTests.Application.AgentManagement;

public class DeleteAgentCommandHandlerTests
{
    private readonly Mock<IAgentRepository> _repoMock = new();
    private readonly DeleteAgentCommandHandler _handler;

    public DeleteAgentCommandHandlerTests()
    {
        _handler = new DeleteAgentCommandHandler(_repoMock.Object);
    }

    [Fact]
    public async Task Handle_WhenAgentNotFound_ShouldThrowKeyNotFoundException()
    {
        _repoMock.Setup(r => r.ExistsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _handler.Handle(new DeleteAgentCommand(Guid.NewGuid()), CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WhenAgentExists_ShouldCallDeleteAsyncOnce()
    {
        var id = Guid.NewGuid();
        _repoMock.Setup(r => r.ExistsAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        await _handler.Handle(new DeleteAgentCommand(id), CancellationToken.None);

        _repoMock.Verify(r => r.DeleteAsync(id, It.IsAny<CancellationToken>()), Times.Once);
    }
}
