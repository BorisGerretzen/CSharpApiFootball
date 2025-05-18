namespace ApiFootball.Models;

public sealed class Standing
{
    public int Rank { get; init; }
    public required Team Team { get; init; }
    public int Points { get; set; }
    public int GoalsDiff { get; init; }
    public required string Group { get; init; }
    public required string Form { get; init; }
    public required string Status { get; init; }
    public required string Description { get; init; }
    public required StandingGames All { get; init; }
    public required StandingGames Home { get; init; }
    public required StandingGames Away { get; init; }
    public DateTimeOffset Update { get; init; }
}

public class StandingGames
{
    public int Played { get; init; }
    public int Win { get; init; }
    public int Draw { get; init; }
    public int Lose { get; init; }
    public required StandingGameGoal Goals { get; init; }
}

public class StandingGameGoal
{
    public int For { get; init; }
    public int Against { get; init; }
}