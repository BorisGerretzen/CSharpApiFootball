namespace ApiFootball.Models;

public sealed class TeamStatisticsFixtures
{
    public TeamStatisticsCount? Played { get; init; }
    public TeamStatisticsCount? Wins { get; init; }
    public TeamStatisticsCount? Draws { get; init; }
    public TeamStatisticsCount? Loses { get; init; }
}