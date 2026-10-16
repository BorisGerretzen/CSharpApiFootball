namespace ApiFootball.Models;

public sealed class PlayerPenaltyStatistics
{
    public int? Won { get; init; }

    [JsonPropertyName("commited")]
    public int? Committed { get; init; }

    public int? Scored { get; init; }
    public int? Missed { get; init; }
    public int? Saved { get; init; }
}