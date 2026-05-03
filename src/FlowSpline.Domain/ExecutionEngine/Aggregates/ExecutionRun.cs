using FlowSpline.Domain.Common;
using FlowSpline.Domain.ExecutionEngine.Enums;
using FlowSpline.Domain.ExecutionEngine.Events;
using FlowSpline.Domain.ExecutionEngine.ValueObjects;

namespace FlowSpline.Domain.ExecutionEngine.Aggregates
{
    public class ExecutionRun : AggregateRoot
    {
        private const int MaxRetries = 3;

        public Guid Id { get; private set; }
        public RunContext Context { get; private set; } = null!;
        public ExecutionStatus Status { get; private set; }
        public DateTimeOffset? StartedAt { get; private set; }
        public DateTimeOffset? CompletedAt { get; private set; }
        public string? FailureReason { get; private set; }
        public int RetryCount { get; private set; }

        private ExecutionRun() { }

        public ExecutionRun(RunContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            Id = Guid.NewGuid();
            Context = context;
            Status = ExecutionStatus.Created;

            AddDomainEvent(new ExecutionCreatedEvent(Id, context.AgentId));
        }

        public void Start()
        {
            GuardStatus(ExecutionStatus.Created, ExecutionStatus.Retrying);

            Status = ExecutionStatus.Running;
            StartedAt = DateTimeOffset.UtcNow;

            AddDomainEvent(new ExecutionStartedEvent(Id));
        }

        public void Complete()
        {
            GuardStatus(ExecutionStatus.Running);

            Status = ExecutionStatus.Completed;
            CompletedAt = DateTimeOffset.UtcNow;

            AddDomainEvent(new ExecutionCompletedEvent(Id));
        }

        public void Fail(string reason)
        {
            GuardStatus(ExecutionStatus.Running);

            if (string.IsNullOrWhiteSpace(reason))
                throw new ArgumentException("Failure reason is required.");

            Status = ExecutionStatus.Failed;
            FailureReason = reason;

            AddDomainEvent(new ExecutionFailedEvent(Id, reason));
        }

        public void RequestApproval()
        {
            GuardStatus(ExecutionStatus.Running);

            Status = ExecutionStatus.WaitingApproval;

            AddDomainEvent(new ApprovalRequestedEvent(Id));
        }

        public void Approve()
        {
            GuardStatus(ExecutionStatus.WaitingApproval);

            Status = ExecutionStatus.Running;
            AddDomainEvent(new ExecutionApprovedEvent(Id));
        }

        public void Retry()
        {
            GuardStatus(ExecutionStatus.Failed);

            if (RetryCount >= MaxRetries)
                throw new InvalidOperationException($"Max retry limit of {MaxRetries} reached.");

            RetryCount++;
            Status = ExecutionStatus.Retrying;

            AddDomainEvent(new ExecutionRetriedEvent(Id, RetryCount));
        }

        private void GuardStatus(params ExecutionStatus[] allowed)
        {
            if (!allowed.Contains(Status))
                throw new InvalidOperationException(
                    $"Cannot transition from {Status}. Allowed states: {string.Join(", ", allowed)}.");
        }
    }
}
