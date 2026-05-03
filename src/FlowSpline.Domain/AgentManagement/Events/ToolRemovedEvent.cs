namespace FlowSpline.Domain.AgentManagement.Events
{
    internal class ToolRemovedEvent
    {
        public Guid AgentId { get; }
        public string ToolName { get; }

        public ToolRemovedEvent(Guid agentId, string toolName)
        {
            AgentId = agentId;
            ToolName = toolName;
        }
    }
}
