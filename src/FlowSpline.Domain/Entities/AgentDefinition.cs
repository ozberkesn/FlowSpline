using FlowSpline.Domain.Common;
using FlowSpline.Domain.Events;
using FlowSpline.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowSpline.Domain.Entities
{
    public class AgentDefinition : AggregateRoot
    {
        private readonly List<Tool> _tools = new();

        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public string SystemPrompt { get; private set; }

        public ModelSettings Model { get; private set; }

        public bool IsActive { get; private set; }

        public IReadOnlyCollection<Tool> Tools => _tools;

        private AgentDefinition() { }

        public AgentDefinition(
            string name,
            string systemPrompt,
            ModelSettings model)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Agent name is required.");

            if (name.Trim().Length < 3)
                throw new ArgumentException("Agent name must be at least 3 characters.");

            if (string.IsNullOrWhiteSpace(systemPrompt))
                throw new ArgumentException("System prompt is required.");

            Id = Guid.NewGuid();
            Name = name.Trim();
            SystemPrompt = systemPrompt.Trim();
            Model = model;
            IsActive = true;

            AddDomainEvent(new AgentCreatedEvent(Id));
        }

        public void BindTool(Tool tool)
        {
            if (_tools.Count >= 10)
                throw new InvalidOperationException("Max 10 tools allowed.");

            if (_tools.Contains(tool))
                throw new InvalidOperationException("Tool already bound.");

            _tools.Add(tool);
        }

        public void RemoveTool(Tool tool)
        {
            _tools.Remove(tool);
        }

        public void ChangePrompt(string newPrompt)
        {
            if (string.IsNullOrWhiteSpace(newPrompt))
                throw new ArgumentException("Prompt cannot be empty.");

            SystemPrompt = newPrompt.Trim();
        }

        public void Activate() => IsActive = true;

        public void Deactivate() => IsActive = false;
    }
}
