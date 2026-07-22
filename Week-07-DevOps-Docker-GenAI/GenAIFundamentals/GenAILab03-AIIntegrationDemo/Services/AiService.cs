using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using GenAILab03_AiIntegrationDemo.Configuration;
using GenAILab03_AiIntegrationDemo.DTOs;
using GenAILab03_AiIntegrationDemo.Interfaces;
using Microsoft.Extensions.Options;

namespace GenAILab03_AiIntegrationDemo.Services;

public sealed class AiService : IAiService
{
    private readonly HttpClient _httpClient;
    private readonly AiOptions _options;

    public AiService(HttpClient httpClient, IOptions<AiOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;
    }

    public async Task<ChatResponse> GetChatResponseAsync(ChatRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Prompt))
        {
            throw new ArgumentException("A prompt is required.", nameof(request));
        }

        if (string.IsNullOrWhiteSpace(_options.ApiKey) || string.IsNullOrWhiteSpace(_options.Endpoint))
        {
            throw new InvalidOperationException("AI options are not configured.");
        }

        var payload = new
        {
            contents = new[]
            {
                new { parts = new[] { new { text = request.Prompt } } }
            }
        };

        using var message = new HttpRequestMessage(HttpMethod.Post, _options.Endpoint);
        message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _options.ApiKey);
        message.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

        using var response = await _httpClient.SendAsync(message, cancellationToken);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        return new ChatResponse { Reply = content };
    }
}
