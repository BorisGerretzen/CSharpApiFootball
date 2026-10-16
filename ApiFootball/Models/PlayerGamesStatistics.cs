namespace ApiFootball.Models;

public sealed class PlayerGamesStatistics
{
    [JsonPropertyName("appearences")]
    public int? Appearances { get; init; }
    public int? Lineups { get; init; }
    public int? Minutes { get; init; }
    public int? Number { get; init; }
    public string? Position { get; init; }
    public string? Rating { get; init; }
    public bool? Captain { get; init; }
}