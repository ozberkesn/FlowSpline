using FlowSpline.Application.ExecutionEngine.GetExecution;
using FlowSpline.Application.ExecutionEngine.Repositories;
using FlowSpline.Domain.ExecutionEngine.Aggregates;
using FlowSpline.Domain.ExecutionEngine.ValueObjects;
using Moq;

namespace FlowSpline.UnitTests.Application.ExecutionEngine;

public class GetExecutionQueryHandlerTests
{
    private readonly Mock<IExecutionRunRepository> _repoMock = new();
    private readonly GetExecutionQueryHandler _handler;

    public GetExecutionQueryHandlerTests()
    {
        _handler = new GetExecutionQueryHandler(_repoMock.Object);
    }

    [Fact]
    public async Task Handle_WhenRunExists_ShouldReturnExecutionRunDto()
    {
        var run = CreateRun();
        _repoMock.Setup(r => r.GetByIdAsync(run.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(run);

        var result = await _handler.Handle(new GetExecutionQuery(run.Id), CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(run.Id, result.Id);
        Assert.Equal("Created", result.Status);
    }

    [Fact]
    public async Task Handle_WhenRunNotFound_ShouldReturnNull()
    {
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((ExecutionRun?)null);

        var result = await _handler.Handle(new GetExecutionQuery(Guid.NewGuid()), CancellationToken.None);

        Assert.Null(result);
    }

    private static ExecutionRun CreateRun() =>
        new(new RunContext(Guid.NewGuid(), "Hello", Guid.NewGuid()));
}
