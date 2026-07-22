# GenAI Lab 03 — AI Integration Demo

## Objective

Build a simple ASP.NET Core Web API endpoint that accepts a prompt and sends it to an AI provider using configuration-based settings.

## Concepts Covered

- ASP.NET Core minimal API
- Dependency Injection
- HttpClient usage
- Configuration management
- DTOs and request/response models
- AI integration

## Folder Structure

```
GenAILab03-AIIntegrationDemo/
├── Controllers/
├── DTOs/
├── Interfaces/
├── Services/
├── Configuration/
├── Program.cs
├── appsettings.json
├── GenAILab03-AIIntegrationDemo.csproj
└── README.md
```

## Prerequisites

- .NET 8 SDK
- A free Gemini or OpenAI-compatible API key

## Installation

Update appsettings.json with your API endpoint and API key before running the application.

## How to Run

```bash
dotnet run
```

Then call the endpoint:

```http
POST /api/ai/chat
Content-Type: application/json

{
  "prompt": "Explain DevOps in simple terms."
}
```

## Expected Output

The API should return an AI-generated response in JSON format.

## Learning Outcomes

- Build a configurable AI integration endpoint
- Follow clean architecture patterns for API services
- Use dependency injection for external service access
