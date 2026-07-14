using System;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Agents.AI;
using FlintWorkflowBackend.Models;

namespace FlintWorkflowBackend.Services;

public static class ChartRecommenderAgentProvider
{
    private const string SystemPrompt = "You are a Data Analysis and Visualization Recommender. Your task is to analyze the user's data schema or sample data and suggest the top 3 best chart types to visualize it. For each suggestion, provide a clear reason why it is a good fit.";

    public static AIAgent Create(IServiceProvider sp)
    {
        var chatClient = sp.GetRequiredService<IChatClient>();

        return chatClient.AsAIAgent(new ChatClientAgentOptions
        {
            Name = "chart-recommender-agent",
            Description = "Analyzes data and suggests appropriate chart types.",
            ChatOptions = new ChatOptions
            {
                Instructions = SystemPrompt,
                ResponseFormat = ChatResponseFormat.ForJsonSchema<ChartRecommendationOutput>()
            }
        });
    }
}
