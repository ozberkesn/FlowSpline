namespace FlowSpline.Domain.AgentManagement.ValueObjects
{
    public class ModelSettings
    {
        public string Provider { get; }
        public string Model { get; }
        public double Temperature { get; }
        public int MaxTokens { get; }

        public ModelSettings(
            string provider,
            string model,
            double temperature,
            int maxTokens)
        {
            if (string.IsNullOrWhiteSpace(provider))
                throw new ArgumentException("Provider required");

            if (string.IsNullOrWhiteSpace(model))
                throw new ArgumentException("Model required");

            if (temperature < 0 || temperature > 2)
                throw new ArgumentException("Invalid temperature");

            if (maxTokens <= 0)
                throw new ArgumentException("Invalid max tokens");

            Provider = provider;
            Model = model;
            Temperature = temperature;
            MaxTokens = maxTokens;
        }
    }
}
