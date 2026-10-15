using ApiFootball.Json;

namespace ApiFootball.Models;

public class FixtureEvent
{
    public required TeamReference Team { get; init; }
    public required FixtureEventTime Time { get; init; }
    public required FixtureEventPlayer Player { get; init; }
    
    [JsonConverter(typeof(FixtureEventAssistConverter))]    
    public required FixtureEventPlayer? Assist { get; init; }
    
    public required string Type { get; init; }
    public required string Detail { get; init; }
    public string? Comments { get; init; }
}