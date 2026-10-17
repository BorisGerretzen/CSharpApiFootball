namespace ApiFootball.Models;

public sealed class PlayerStatisticsResponse
{
    public PlayerProfile? Player { get; init; }
    public List<PlayerSeasonStatistics>? Statistics { get; init; }
}