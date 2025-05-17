namespace ApiFootball.Models;

public class Fixtures
{
    public bool Events { get; init; }
    public bool Lineups { get; init; }

    [JsonPropertyName("statistics_fixtures")]
    public bool StatisticsFixtures { get; init; }

    [JsonPropertyName("statistics_players")]
    public bool StatisticsPlayers { get; init; }
}