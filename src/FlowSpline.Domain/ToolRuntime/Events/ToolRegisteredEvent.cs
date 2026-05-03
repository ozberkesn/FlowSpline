namespace FlowSpline.Domain.ToolRuntime.Events
{
    internal class ToolRegisteredEvent
    {
        public Guid ToolId { get; }
        public string Name { get; }

        public ToolRegisteredEvent(Guid toolId, string name)
        {
            ToolId = toolId;
            Name = name;
        }
    }
}
