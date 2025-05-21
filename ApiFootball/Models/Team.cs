namespace ApiFootball.Models;

public sealed class Team
{
    public int Id { get; init; }
    public required string Name { get; init; }
    public string? Code { get; init; }
    public string? Country { get; init; }
    public int? Founded { get; init; }
    public bool? National { get; init; }
    public bool? Winner { get; init; }
    public required string Logo { get; init; }
}