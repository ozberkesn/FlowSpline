using System;
using System.Collections.Generic;
using System.Text;

namespace FlowSpline.Domain.Entities
{
    public class AgentDefinition
    {
        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public string SystemPrompt { get; private set; }

        public string ModelProvider { get; private set; }

        public bool IsActive { get; private set; }

        private AgentDefinition()
        {
        }

        public AgentDefinition(
     string name,
     string systemPrompt,
     string modelProvider)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Agent name is required.");

            if (name.Trim().Length < 3)
                throw new ArgumentException(
                    "Agent name must be at least 3 characters.");

            if (string.IsNullOrWhiteSpace(systemPrompt))
                throw new ArgumentException(
                    "System prompt is required.");

            if (string.IsNullOrWhiteSpace(modelProvider))
                throw new ArgumentException(
                    "Model provider is required.");

            Id = Guid.NewGuid();
            Name = name.Trim();
            SystemPrompt = systemPrompt.Trim();
            ModelProvider = modelProvider.Trim();
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        public void Activate()
        {
            IsActive = true;
        }

        public void ChangePrompt(string newPrompt)
        {
            if (string.IsNullOrWhiteSpace(newPrompt))
                throw new ArgumentException("Prompt cannot be empty.");

            SystemPrompt = newPrompt.Trim();
        }
    }
}
