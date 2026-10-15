namespace ApiFootball.Models;

public class FixtureLineupTeam
{
    public int Id { get; init; }
    public required string Name { get; init; }
    public required string Logo { get; init; }
    
    public required TeamOutfitColors Colors { get; init; }
}