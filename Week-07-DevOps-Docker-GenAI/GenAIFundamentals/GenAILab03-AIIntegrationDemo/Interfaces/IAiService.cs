using GenAILab03_AiIntegrationDemo.DTOs;

namespace GenAILab03_AiIntegrationDemo.Interfaces;

public interface IAiService
{
    Task<ChatResponse> GetChatResponseAsync(ChatRequest request, CancellationToken cancellationToken);
}
