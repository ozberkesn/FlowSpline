using System;
using System.Collections.Generic;
using System.Text;

namespace FlowSpline.Domain.Events
{
    internal class AgentCreatedEvent
    {
        public Guid AgentId { get; }

        public AgentCreatedEvent(Guid agentId)
        {
            AgentId = agentId;
        }
    }
}
