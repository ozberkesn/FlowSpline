using FlowSpline.Domain.Common;
using FlowSpline.Domain.ToolRuntime.Events;
using FlowSpline.Domain.ToolRuntime.ValueObjects;

namespace FlowSpline.Domain.ToolRuntime.Aggregates
{
    public class ToolDefinition : AggregateRoot
    {
        private static readonly System.Text.RegularExpressions.Regex SlugPattern =
            new(@"^[a-z0-9_-]+$", System.Text.RegularExpressions.RegexOptions.Compiled);

        public Guid Id { get; private set; }
        public string Name { get; private set; } = null!;
        public string Description { get; private set; } = null!;
        public ToolSchema Schema { get; private set; } = null!;
        public bool IsEnabled { get; private set; }

        private ToolDefinition() { }

        public ToolDefinition(string name, string description, ToolSchema schema)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Tool name is required.");

            if (!SlugPattern.IsMatch(name))
                throw new ArgumentException("Tool name must be lowercase alphanumeric with hyphens or underscores only.");

            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Tool description is required.");

            ArgumentNullException.ThrowIfNull(schema);

            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            Schema = schema;
            IsEnabled = true;

            AddDomainEvent(new ToolRegisteredEvent(Id, Name));
        }

        public void Enable()
        {
            if (IsEnabled) return;
            IsEnabled = true;
            AddDomainEvent(new ToolEnabledEvent(Id));
        }

        public void Disable()
        {
            if (!IsEnabled) return;
            IsEnabled = false;
            AddDomainEvent(new ToolDisabledEvent(Id));
        }

        public void UpdateDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Tool description is required.");

            Description = description;
            AddDomainEvent(new ToolDescriptionUpdatedEvent(Id));
        }

        public void UpdateSchema(ToolSchema schema)
        {
            ArgumentNullException.ThrowIfNull(schema);
            Schema = schema;
            AddDomainEvent(new ToolSchemaUpdatedEvent(Id));
        }
    }
}
