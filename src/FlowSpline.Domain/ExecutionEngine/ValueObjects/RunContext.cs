namespace FlowSpline.Domain.ExecutionEngine.ValueObjects
{
    public class RunContext
    {
        public Guid AgentId { get; }
        public string Input { get; }
        public Guid SessionId { get; }

        public RunContext(Guid agentId, string input, Guid sessionId)
        {
            if (agentId == Guid.Empty)
                throw new ArgumentException("Agent ID is required.");

            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentException("Input is required.");

            if (sessionId == Guid.Empty)
                throw new ArgumentException("Session ID is required.");

            AgentId = agentId;
            Input = input;
            SessionId = sessionId;
        }
    }
}
