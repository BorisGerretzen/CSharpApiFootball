namespace ApiFootball.Models;

public sealed class TeamStatisticsScore
{
    public string? Home { get; init; }
    public string? Away { get; init; }
    public int? Wins { get; init; }
    public int? Draws { get; init; }
    public int? Loses { get; init; }
    public string? For { get; init; }
    public string? Against { get; init; }
}