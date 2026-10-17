namespace ApiFootball.Models;

public class FixtureLineupPlayer
{
    public required FixtureLineupPlayerDetails Player { get; init; }
}

public class FixtureLineupPlayerDetails
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required int Number { get; init; }
    public required string Pos { get; init; }
    public string? Grid { get; init; }
}