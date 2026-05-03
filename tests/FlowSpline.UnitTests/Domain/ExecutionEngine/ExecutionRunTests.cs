using FlowSpline.Domain.ExecutionEngine.Aggregates;
using FlowSpline.Domain.ExecutionEngine.Enums;
using FlowSpline.Domain.ExecutionEngine.ValueObjects;

namespace FlowSpline.UnitTests.Domain.ExecutionEngine;

public class ExecutionRunTests
{
    [Fact]
    public void CreateRun_WithValidContext_ShouldBeInCreatedState()
    {
        var run = CreateRun();

        Assert.NotEqual(Guid.Empty, run.Id);
        Assert.Equal(ExecutionStatus.Created, run.Status);
        Assert.Null(run.StartedAt);
        Assert.Null(run.CompletedAt);
        Assert.Equal(0, run.RetryCount);
    }

    [Fact]
    public void Start_WhenCreated_ShouldTransitionToRunning()
    {
        var run = CreateRun();

        run.Start();

        Assert.Equal(ExecutionStatus.Running, run.Status);
        Assert.NotNull(run.StartedAt);
    }

    [Fact]
    public void Start_WhenRunning_ShouldThrow()
    {
        var run = CreateRun();
        run.Start();

        Assert.Throws<InvalidOperationException>(() => run.Start());
    }

    [Fact]
    public void Complete_WhenRunning_ShouldTransitionToCompleted()
    {
        var run = CreateRun();
        run.Start();

        run.Complete();

        Assert.Equal(ExecutionStatus.Completed, run.Status);
        Assert.NotNull(run.CompletedAt);
    }

    [Fact]
    public void Complete_WhenCreated_ShouldThrow()
    {
        var run = CreateRun();

        Assert.Throws<InvalidOperationException>(() => run.Complete());
    }

    [Fact]
    public void Fail_WhenRunning_ShouldTransitionToFailed()
    {
        var run = CreateRun();
        run.Start();

        run.Fail("LLM timeout");

        Assert.Equal(ExecutionStatus.Failed, run.Status);
        Assert.Equal("LLM timeout", run.FailureReason);
    }

    [Fact]
    public void RequestApproval_WhenRunning_ShouldTransitionToWaitingApproval()
    {
        var run = CreateRun();
        run.Start();

        run.RequestApproval();

        Assert.Equal(ExecutionStatus.WaitingApproval, run.Status);
    }

    [Fact]
    public void Approve_WhenWaitingApproval_ShouldReturnToRunningAndRaiseEvent()
    {
        var run = CreateRun();
        run.Start();
        run.RequestApproval();
        run.ClearDomainEvents();

        run.Approve();

        Assert.Equal(ExecutionStatus.Running, run.Status);
        Assert.Single(run.DomainEvents);
    }

    [Fact]
    public void Retry_WhenFailed_ShouldTransitionToRetrying()
    {
        var run = CreateRun();
        run.Start();
        run.Fail("timeout");

        run.Retry();

        Assert.Equal(ExecutionStatus.Retrying, run.Status);
        Assert.Equal(1, run.RetryCount);
    }

    [Fact]
    public void Retry_BeyondMaxRetries_ShouldThrow()
    {
        var run = CreateRun();

        for (int i = 0; i < 3; i++)
        {
            run.Start();
            run.Fail("timeout");
            run.Retry();
        }

        run.Start();
        run.Fail("timeout");

        Assert.Throws<InvalidOperationException>(() => run.Retry());
    }

    [Fact]
    public void CreateRun_ShouldRaiseDomainEvent()
    {
        var run = CreateRun();

        Assert.Single(run.DomainEvents);
    }

    [Fact]
    public void FullHappyPath_ShouldRaiseCorrectEvents()
    {
        var run = CreateRun();
        run.Start();
        run.Complete();

        Assert.Equal(3, run.DomainEvents.Count);
    }

    private static ExecutionRun CreateRun()
    {
        var context = new RunContext(Guid.NewGuid(), "Hello", Guid.NewGuid());
        return new ExecutionRun(context);
    }
}
