namespace FlowSpline.Domain.ToolRuntime.Events
{
    internal class ToolDescriptionUpdatedEvent
    {
        public Guid ToolId { get; }

        public ToolDescriptionUpdatedEvent(Guid toolId)
        {
            ToolId = toolId;
        }
    }
}
