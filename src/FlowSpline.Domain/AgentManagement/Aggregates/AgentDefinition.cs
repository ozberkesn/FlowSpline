using FlowSpline.Domain.AgentManagement.Events;
using FlowSpline.Domain.AgentManagement.ValueObjects;
using FlowSpline.Domain.Common;

namespace FlowSpline.Domain.AgentManagement.Aggregates
{
    public class AgentDefinition : AggregateRoot
    {
        private readonly List<Tool> _tools = new();

        public Guid Id { get; private set; }
        public string Name { get; private set; } = null!;
        public string SystemPrompt { get; private set; } = null!;
        public ModelSettings Model { get; private set; } = null!;
        public bool IsActive { get; private set; }
        public IReadOnlyCollection<Tool> Tools => _tools;

        private AgentDefinition() { }

        public AgentDefinition(string name, string systemPrompt, ModelSettings model)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Agent name is required.");

            var trimmedName = name.Trim();
            if (trimmedName.Length < 3)
                throw new ArgumentException("Agent name must be at least 3 characters.");

            if (string.IsNullOrWhiteSpace(systemPrompt))
                throw new ArgumentException("System prompt is required.");

            ArgumentNullException.ThrowIfNull(model);

            Id = Guid.NewGuid();
            Name = trimmedName;
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
            AddDomainEvent(new ToolBoundEvent(Id, tool.Name));
        }

        public void RemoveTool(Tool tool)
        {
            if (!_tools.Contains(tool))
                throw new InvalidOperationException("Tool is not bound to this agent.");

            _tools.Remove(tool);
            AddDomainEvent(new ToolRemovedEvent(Id, tool.Name));
        }

        public void ChangePrompt(string newPrompt)
        {
            if (string.IsNullOrWhiteSpace(newPrompt))
                throw new ArgumentException("Prompt cannot be empty.");

            SystemPrompt = newPrompt.Trim();
            AddDomainEvent(new PromptChangedEvent(Id));
        }

        public void Activate()
        {
            if (IsActive) return;
            IsActive = true;
            AddDomainEvent(new AgentActivatedEvent(Id));
        }

        public void Deactivate()
        {
            if (!IsActive) return;
            IsActive = false;
            AddDomainEvent(new AgentDeactivatedEvent(Id));
        }
    }
}
