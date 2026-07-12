using System;
using System.ClientModel;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenAI;
using FlintWorkflowBackend.Configuration;

namespace FlintWorkflowBackend.Services;

public static class ChatClientProvider
{
    public static IChatClient Create(IServiceProvider sp)
    {
        var llmSettings = sp.GetRequiredService<IOptions<LlmSettings>>().Value;
        var activeSettings = llmSettings.GetActiveSettings();
        var credential = sp.GetRequiredService<ApiKeyCredential>();

        var clientOptions = new OpenAIClientOptions
        {
            Endpoint = new Uri(activeSettings.Endpoint)
        };

        var openAiClient = new OpenAIClient(credential, clientOptions);

        return openAiClient
            .GetChatClient(activeSettings.Model)
            .AsIChatClient();
    }
}
