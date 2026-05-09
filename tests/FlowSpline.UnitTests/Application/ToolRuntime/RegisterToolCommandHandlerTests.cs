using FlowSpline.Application.ToolRuntime.RegisterTool;
using FlowSpline.Application.ToolRuntime.Repositories;
using FlowSpline.Domain.ToolRuntime.Aggregates;
using Moq;

namespace FlowSpline.UnitTests.Application.ToolRuntime;

public class RegisterToolCommandHandlerTests
{
    private readonly Mock<IToolDefinitionRepository> _repoMock = new();
    private readonly RegisterToolCommandHandler _handler;

    public RegisterToolCommandHandlerTests()
    {
        _handler = new RegisterToolCommandHandler(_repoMock.Object);
    }

    [Fact]
    public async Task Handle_WhenNameAlreadyExists_ShouldThrowInvalidOperationException()
    {
        _repoMock.Setup(r => r.ExistsByNameAsync("search", It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _handler.Handle(new RegisterToolCommand("search", "A search tool", null, null), CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldReturnNonEmptyGuid()
    {
        _repoMock.Setup(r => r.ExistsByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var id = await _handler.Handle(new RegisterToolCommand("search", "A search tool", null, null), CancellationToken.None);

        Assert.NotEqual(Guid.Empty, id);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldCallAddAsyncOnce()
    {
        _repoMock.Setup(r => r.ExistsByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        await _handler.Handle(new RegisterToolCommand("search", "A search tool", null, null), CancellationToken.None);

        _repoMock.Verify(r => r.AddAsync(It.IsAny<ToolDefinition>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
