namespace ApiFootball.Models;

public sealed class OddsValue
{
    public JsonElement? Value { get; init; }
    public string? Odd { get; init; }
    public string? Handicap { get; init; }
    public bool? Main { get; init; }
    public bool? Suspended { get; init; }
}