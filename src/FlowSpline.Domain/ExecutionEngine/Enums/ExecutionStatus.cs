namespace FlowSpline.Domain.ExecutionEngine.Enums
{
    public enum ExecutionStatus
    {
        Created,
        Running,
        WaitingApproval,
        Completed,
        Failed,
        Retrying
    }
}
