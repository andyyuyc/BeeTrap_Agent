
using System;
using System.IO;
using System.Text.Json;
using System.Net.Http;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;

public class ARCursorStateData
{
    public int click { get; set; }
    public bool selectPanelActive { get; set; }
    public int CurrentSelectedFLowerID { get; set; }
    public int experience { get; set; }
    public string lastButtonPressed { get; set; }
}

public class BeeAgent
{
    private readonly string apiKey;
    private readonly string apiUrl;
    private readonly string model;
    private string lastStateJson = null;
    private readonly HttpClient httpClient = new();

    public BeeAgent(string configPath)
    {
        var config = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(configPath));
        apiKey = config["apiKey"];
        apiUrl = config["apiUrl"];
        model = config["model"];
    }

    public bool ShouldProvideHint(ARCursorStateData state)
    {
        string currentStateJson = JsonSerializer.Serialize(state);
        if (currentStateJson == lastStateJson) return false;
        if (state.click == 0 || !state.selectPanelActive) return true;
        lastStateJson = currentStateJson;
        return false;
    }

    public async Task<string> ProvideHintAsync(ARCursorStateData state)
    {
        var messages = new[]
        {
            new { role = "system", content = "You are Bip Buzzley, a friendly bee guide who helps the player navigate through the game. Provide short and encouraging hints if the player seems stuck or confused." },
            new { role = "user", content = $"The game state is: {state}. The player seems stuck. Provide a helpful hint." }
        };

        var payload = new { model = model, messages = messages };
        string jsonBody = JsonSerializer.Serialize(payload);

        var request = new HttpRequestMessage(HttpMethod.Post, apiUrl);
        request.Headers.Add("Authorization", $"Bearer {apiKey}");
        request.Content = new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json");

        var response = await httpClient.SendAsync(request);
        string responseString = await response.Content.ReadAsStringAsync();

        using JsonDocument doc = JsonDocument.Parse(responseString);
        try
        {
            return doc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();
        }
        catch
        {
            return "⚠️ Could not parse GPT response: " + responseString;
        }
    }
}

class AgentWorker
{
    public static async Task Main()
    {
        var agent = new BeeAgent("appsettings.json");

        string logDir = "data_logs";
        string processedDir = "processed_logs";
        Directory.CreateDirectory(logDir);
        Directory.CreateDirectory(processedDir);

        Console.WriteLine("🧠 AgentWorker started...");

        while (true)
        {
            var files = Directory.GetFiles(logDir, "*.json");
            foreach (string file in files)
            {
                try
                {
                    string json = File.ReadAllText(file);
                    var state = JsonSerializer.Deserialize<ARCursorStateData>(json);

                    if (agent.ShouldProvideHint(state))
                    {
                        string hint = await agent.ProvideHintAsync(state);
                        Console.WriteLine($"💡 Hint from {Path.GetFileName(file)}: {hint}");
                    }

                    string dest = Path.Combine(processedDir, Path.GetFileName(file));
                    File.Move(file, dest);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("❌ Error: " + ex.Message);
                }
            }

            await Task.Delay(1000); // check every second
        }
    }
}
