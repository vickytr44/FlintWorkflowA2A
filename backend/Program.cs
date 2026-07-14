using System.ClientModel;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;
using OpenAI;
using A2A;
using A2A.AspNetCore;
using Microsoft.Agents.AI;
using Microsoft.Agents.AI.Hosting;
using FlintWorkflowBackend.Configuration;
using FlintWorkflowBackend.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. Configure the Application Configuration sources
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
    .AddJsonFile("appsettings.development.json", optional: true, reloadOnChange: false)
    .AddEnvironmentVariables();

// 2. Configure LLM settings from appsettings
builder.Services.Configure<LlmSettings>(builder.Configuration.GetSection(LlmSettings.SectionName));

// 3. Register OpenAI API Key Credentials
builder.Services.AddSingleton<ApiKeyCredential>(sp =>
{
    var llmSettings = sp.GetRequiredService<IOptions<LlmSettings>>().Value;
    var activeSettings = llmSettings.GetActiveSettings();

    if (string.IsNullOrWhiteSpace(activeSettings.ApiKey) || activeSettings.ApiKey.StartsWith("YOUR_"))
    {
        throw new InvalidOperationException(
            $"LLM API key not configured for provider '{llmSettings.Provider}'. " +
            $"Please set 'Llm:{llmSettings.Provider}:ApiKey' in appsettings.development.json.");
    }

    return new ApiKeyCredential(activeSettings.ApiKey);
});

// 4. Register the IChatClient configured for the active provider
builder.Services.AddSingleton<IChatClient>(sp => FlintWorkflowBackend.Services.ChatClientProvider.Create(sp));

builder.Services.AddKeyedSingleton<AIAgent>("flint-agent", FlintWorkflowBackend.Services.FlintAgentProvider.Create);
builder.Services.AddKeyedSingleton<AIAgent>("chart-recommender-agent", (sp, key) => FlintWorkflowBackend.Services.ChartRecommenderAgentProvider.Create(sp));

// 6. Register the A2A Server for the agents
builder.AddA2AServer("flint-agent");
builder.AddA2AServer("chart-recommender-agent");

var app = builder.Build();

// 7. Setup Routing
app.UseRouting();

// 8. Map A2A endpoints
app.MapA2AHttpJson("flint-agent", "/a2a/flint-agent");
app.MapA2AHttpJson("chart-recommender-agent", "/a2a/chart-recommender-agent");

// 9. Map Well-Known Agent Card for A2A Discovery
app.MapWellKnownAgentCard(new AgentCard
{
    Name = "FlintAgent",
    Description = "Translates natural language descriptions of data into Flint chart specifications.",
    Version = "1.0.0",
    DefaultInputModes = new List<string> { "text" },
    DefaultOutputModes = new List<string> { "text" },
    Skills = new List<A2A.AgentSkill>
    {
        new A2A.AgentSkill
        {
            Id = "recommend_chart_types",
            Name = "Chart Recommender",
            Description = "Analyzes data and suggests appropriate chart types."
        }
    },
    SupportedInterfaces = new List<AgentInterface>
    {
        new AgentInterface
        {
            Url = "http://localhost:5000/a2a/flint-agent",
            ProtocolBinding = ProtocolBindingNames.HttpJson,
            ProtocolVersion = "1.0"
        }
    }
}, "");

app.Lifetime.ApplicationStarted.Register(() =>
{
    Console.WriteLine("=== MAPPED ENDPOINTS ON STARTUP ===");
    var dataSource = app.Services.GetRequiredService<EndpointDataSource>();
    foreach (var endpoint in dataSource.Endpoints)
    {
        Console.WriteLine($"Endpoint: {endpoint.DisplayName}");
        if (endpoint is RouteEndpoint routeEndpoint)
        {
            Console.WriteLine($"  Route Pattern: {routeEndpoint.RoutePattern.RawText}");
        }
    }
    Console.WriteLine("=== END MAPPED ENDPOINTS ===");
    
    var llmSettings = app.Services.GetRequiredService<IOptions<LlmSettings>>().Value;
    var activeSettings = llmSettings.GetActiveSettings();

    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("\n🚀 Flint A2A Agent Backend is running at http://localhost:5000");
    Console.WriteLine($"   Active LLM Provider: {llmSettings.Provider} (Model: {activeSettings.Model})");
    Console.WriteLine("   Exposing A2A HTTP JSON endpoint at: http://localhost:5000/a2a/flint-agent");
    Console.WriteLine("   Exposing A2A Agent Card endpoint at: http://localhost:5000/.well-known/agent-card.json");
    Console.ResetColor();
});

await app.RunAsync();
