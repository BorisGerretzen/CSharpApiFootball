namespace ApiFootball.Models;

public sealed class TeamStatisticsGoals
{
    public TeamStatisticsGoalSplit? For { get; init; }
    public TeamStatisticsGoalSplit? Against { get; init; }
}