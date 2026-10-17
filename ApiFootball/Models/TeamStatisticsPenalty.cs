namespace ApiFootball.Models;

public sealed class TeamStatisticsPenalty
{
    public TeamStatisticsPenaltyCount? Scored { get; init; }
    public TeamStatisticsPenaltyCount? Missed { get; init; }
}