namespace ApiFootball.Models;

public sealed class TeamStatisticsBiggest
{
    public TeamStatisticsScore? Streak { get; init; }
    public TeamStatisticsScore? Wins { get; init; }
    public TeamStatisticsScore? Loses { get; init; }
    public TeamStatisticsBiggestGoals? Goals { get; init; }
}