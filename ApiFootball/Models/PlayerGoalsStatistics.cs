namespace ApiFootball.Models;

public sealed class PlayerGoalsStatistics
{
    public int? Total { get; init; }
    public int? Conceded { get; init; }
    public int? Assists { get; init; }
    public int? Saves { get; init; }
}