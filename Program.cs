using ConversationMemoryManager.Memory;
using ConversationMemoryManager.Models;
using ConversationMemoryManager.Services;

Console.Title = "C# Conversation Memory Manager";

var memory = new ConversationStore();
using var aiService = new OllamaChatService();

string filePath = Path.Combine(
    Environment.GetFolderPath(
        Environment.SpecialFolder.LocalApplicationData),
    "ConversationMemoryManager",
    "conversation.json");

Console.WriteLine("==================================");
Console.WriteLine("   CONVERSATION MEMORY MANAGER");
Console.WriteLine("   Visual Studio 2026");
Console.WriteLine("   Ollama + Qwen 3.5");
Console.WriteLine("==================================");
Console.WriteLine();

Console.WriteLine(
    "Commands: history, summary, summarize, save, load, clear, exit");

while (true)
{
    Console.WriteLine();
    Console.Write("You: ");

    string? input = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(input))
        continue;

    string command = input.Trim().ToLowerInvariant();

    if (command == "exit")
        break;

    try
    {
        switch (command)
        {
            case "history":
                Console.WriteLine("Conversation History:");

                foreach (var message in memory.GetMessages())
                {
                    Console.WriteLine(
                        $"{message.Role}: {message.Content}");
                }
                continue;

            case "summary":
                Console.WriteLine("Conversation Summary:");
                Console.WriteLine(
                    string.IsNullOrWhiteSpace(memory.Summary)
                        ? "(No summary available)"
                        : memory.Summary);
                continue;

            case "summarize":
                var older = memory.GetOlderMessages(6);

                if (older.Count == 0)
                {
                    Console.WriteLine(
                        "Not enough older messages to summarize.");
                    continue;
                }

                Console.WriteLine("Summarizing older messages...");

                string newSummary =
                    await aiService.SummarizeAsync(
                        memory.Summary, older);

                Console.WriteLine();
                Console.WriteLine("Proposed Summary:");
                Console.WriteLine(newSummary);
                Console.WriteLine();
                Console.Write(
                    "Replace older messages with this summary? (Y/N): ");

                if (Console.ReadLine()?.Trim()
                    .Equals("Y", StringComparison.OrdinalIgnoreCase)
                    == true)
                {
                    memory.Compact(newSummary, 6);
                    Console.WriteLine("Memory compacted.");
                }
                else
                {
                    Console.WriteLine("Memory unchanged.");
                }
                continue;

            case "save":
                Directory.CreateDirectory(
                    Path.GetDirectoryName(filePath)!);

                memory.Save(filePath);
                Console.WriteLine(
                    $"Conversation saved to: {filePath}");
                continue;

            case "load":
                if (File.Exists(filePath))
                {
                    memory.Load(filePath);
                    Console.WriteLine("Conversation loaded.");
                }
                else
                {
                    Console.WriteLine(
                        "No saved conversation found.");
                }
                continue;

            case "clear":
                memory.Clear();
                Console.WriteLine("Conversation cleared.");
                continue;
        }

        var messages = memory.GetContext(6);
        messages.Add(new ChatMessage("user", input));

        Console.WriteLine("Qwen is thinking...");

        string answer =
            await aiService.GenerateAsync(messages);

        memory.Add("user", input);
        memory.Add("assistant", answer);

        Console.WriteLine();
        Console.WriteLine("Assistant:");
        Console.WriteLine(answer);
    }
    catch (HttpRequestException ex)
    {
        Console.WriteLine("Ollama API error: " + ex.Message);
    }
    catch (TaskCanceledException)
    {
        Console.WriteLine("The request timed out.");
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error: " + ex.Message);
    }
}

Console.WriteLine("Goodbye!");
