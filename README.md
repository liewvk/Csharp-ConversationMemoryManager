# Conversation Memory Manager

A C# console application that demonstrates lightweight conversation memory management for an AI agent using local persistent state and Ollama-based chat generation.

This project is based on the idea of keeping recent conversational context, summarizing older exchanges, and compacting memory to reduce token usage over time while preserving useful history.

## Features

- Chat with an Ollama-hosted model (for example, Qwen 3.5)
- Maintain a conversation history in memory
- Keep a rolling window of recent context for prompts
- Summarize older messages and compact history
- Save and load conversation state from a local JSON file
- Clear conversation data when needed
- Simple command-driven console UI

## Project Structure

- `Program.cs` – main console loop and command handling
- `Memory/` – conversation storage and memory management logic
- `Models/` – message and conversation-related model definitions
- `Services/` – chat and summarization integration with Ollama

## Requirements

- .NET 10 SDK
- Ollama installed and running locally
- A model pulled in Ollama, such as `qwen3.5`

## Quick Start

1. Install the .NET 10 SDK.
2. Install and run Ollama.
3. Pull a model, for example:

```bash
ollama pull qwen3.5
```

4. Build and run the project:

```bash
dotnet build

dotnet run
```

## Commands

When the console app is running, the following commands are available:

- `history` – show the current conversation history
- `summary` – display the current summary
- `summarize` – summarize older messages and optionally compact the memory
- `save` – save the current conversation to a local JSON file
- `load` – restore the saved conversation from disk
- `clear` – clear the conversation memory
- `exit` – exit the application

## Storage

The application stores conversation data in a local user profile directory, under:

```text
%LOCALAPPDATA%\ConversationMemoryManager\conversation.json
```

## Notes

This repository is a practical example of memory management in a C# AI agent workflow. It focuses on a very small, local-first implementation that can be adapted for larger agent systems.

## License

This project does not currently declare a license file.
