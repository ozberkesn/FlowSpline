namespace FlowSpline.Domain.AgentManagement.Events
{
    internal class PromptChangedEvent
    {
        public Guid AgentId { get; }

        public PromptChangedEvent(Guid agentId)
        {
            AgentId = agentId;
        }
    }
}
