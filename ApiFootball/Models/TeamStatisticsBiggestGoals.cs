namespace ApiFootball.Models;

public sealed class TeamStatisticsBiggestGoals
{
    public TeamStatisticsBiggestGoalSide? For { get; init; }
    public TeamStatisticsBiggestGoalSide? Against { get; init; }
}