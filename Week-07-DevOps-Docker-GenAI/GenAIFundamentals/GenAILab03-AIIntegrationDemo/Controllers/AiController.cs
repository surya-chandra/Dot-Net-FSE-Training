using GenAILab03_AiIntegrationDemo.DTOs;
using GenAILab03_AiIntegrationDemo.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GenAILab03_AiIntegrationDemo.Controllers;

[ApiController]
[Route("api/ai")]
public sealed class AiController : ControllerBase
{
    private readonly IAiService _aiService;

    public AiController(IAiService aiService)
    {
        _aiService = aiService;
    }

    [HttpPost("chat")]
    public async Task<ActionResult<ChatResponse>> Chat([FromBody] ChatRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _aiService.GetChatResponseAsync(request, cancellationToken);
            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Problem(ex.Message);
        }
    }
}
