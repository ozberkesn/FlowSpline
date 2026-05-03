namespace FlowSpline.Domain.ToolRuntime.Events
{
    internal class ToolDisabledEvent
    {
        public Guid ToolId { get; }

        public ToolDisabledEvent(Guid toolId)
        {
            ToolId = toolId;
        }
    }
}
