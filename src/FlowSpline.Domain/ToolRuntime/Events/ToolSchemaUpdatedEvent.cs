namespace FlowSpline.Domain.ToolRuntime.Events
{
    internal class ToolSchemaUpdatedEvent
    {
        public Guid ToolId { get; }

        public ToolSchemaUpdatedEvent(Guid toolId)
        {
            ToolId = toolId;
        }
    }
}
