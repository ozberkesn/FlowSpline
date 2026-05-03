namespace FlowSpline.Domain.ToolRuntime.Events
{
    internal class ToolEnabledEvent
    {
        public Guid ToolId { get; }

        public ToolEnabledEvent(Guid toolId)
        {
            ToolId = toolId;
        }
    }
}
