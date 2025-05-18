namespace ApiFootball.Models;

public class Coverage
{
    public required Fixtures Fixtures { get; init; }
    public bool Standings { get; init; }
    public bool Players { get; init; }
    [JsonPropertyName("top_scorers")] public bool TopScorers { get; init; }
    [JsonPropertyName("top_assists")] public bool TopAssists { get; init; }
    [JsonPropertyName("top_cards")] public bool TopCards { get; init; }
    public bool Injuries { get; init; }
    public bool Predictions { get; init; }
    public bool Odds { get; init; }
}