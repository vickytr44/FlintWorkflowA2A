using System;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Agents.AI;
using FlintWorkflowBackend.Models;

namespace FlintWorkflowBackend.Services;

public static class FlintAgentProvider
{
    private const string SystemPrompt = "You are a specialized Data Visualization Agent. Your sole responsibility is to translate user descriptions of data and charting intentions into a valid Flint 'ChartAssemblyInput' JSON specification. Important: You must strictly use ONLY the encoding channels defined for the requested chart type in the following registry:\n\n" + FlintWorkflowBackend.Constants.ChartReference.RegistryJson;

    public static AIAgent Create(IServiceProvider sp, object? key)
    {
        var chatClient = sp.GetRequiredService<IChatClient>();

        var recommenderAgent = ChartRecommenderAgentProvider.Create(sp);

        return chatClient.AsAIAgent(new ChatClientAgentOptions
        {
            Name = "flint-agent",
            Description = "Translates natural language descriptions of data into Flint chart specifications.",
            ChatOptions = new ChatOptions
            {
                Instructions = SystemPrompt,
                ResponseFormat = ChatResponseFormat.ForJsonSchema<ChartAssemblyInput>(),
                Tools = [recommenderAgent.AsAIFunction()]
            }
        });
    }
}
