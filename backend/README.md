# Flint Workflow Backend

Welcome to the **Flint Workflow Backend** repository. This service powers the core logic and AI agent functionalities for Flint Workflows.

## Architecture & A2A Protocol

This backend is designed following the **Agent to Agent (A2A) protocol**. It does not serve traditional REST endpoints for a direct user interface; rather, it exposes an intelligent agent capable of communicating with other agents in a distributed system.

> [!NOTE]
> **Integration Context:** There is another repository called `timesheet-copilot-app` which acts as the consumer for this service. The `timesheet-copilot-app` uses this A2A agent to process workflows, retrieve data, and fulfill its copilot capabilities.

## Tech Stack
- **Framework:** .NET (C#)
- **Agent Protocol:** A2A (Agent to Agent)

## Getting Started

To run the backend locally:

1. Restore dependencies:
   ```bash
   dotnet restore
   ```
2. Run the application:
   ```bash
   dotnet run
   ```

*(Ensure you have your environment variables and configuration files like `appsettings.Development.json` properly set up before starting the application.)*
