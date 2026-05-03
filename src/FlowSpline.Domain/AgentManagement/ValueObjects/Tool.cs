namespace FlowSpline.Domain.AgentManagement.ValueObjects
{
    public class Tool : IEquatable<Tool>
    {
        public string Name { get; }

        public Tool(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Tool name is required.");

            Name = name.Trim();
        }

        public bool Equals(Tool? other)
        {
            if (other is null) return false;
            return Name == other.Name;
        }

        public override bool Equals(object? obj) => Equals(obj as Tool);

        public override int GetHashCode() => Name.GetHashCode();
    }
}
