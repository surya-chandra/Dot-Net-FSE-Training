namespace GenAILab03_AiIntegrationDemo.Configuration;

public sealed class AiOptions
{
    public const string SectionName = "AiOptions";

    public string ApiKey { get; set; } = string.Empty;

    public string Endpoint { get; set; } = string.Empty;
}
