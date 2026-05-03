namespace FlowSpline.Domain.AgentManagement.Events
{
    internal class ToolBoundEvent
    {
        public Guid AgentId { get; }
        public string ToolName { get; }

        public ToolBoundEvent(Guid agentId, string toolName)
        {
            AgentId = agentId;
            ToolName = toolName;
        }
    }
}
