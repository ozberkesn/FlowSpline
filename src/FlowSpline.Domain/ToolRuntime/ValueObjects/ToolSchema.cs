namespace FlowSpline.Domain.ToolRuntime.ValueObjects
{
    public class ToolSchema
    {
        public string? InputSchema { get; }
        public string? OutputSchema { get; }

        public ToolSchema(string? inputSchema, string? outputSchema)
        {
            InputSchema = inputSchema;
            OutputSchema = outputSchema;
        }
    }
}
