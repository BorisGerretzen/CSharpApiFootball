namespace ApiFootball.Models;

public sealed class InjuryPlayer
{
    public int? Id { get; init; }
    public string? Name { get; init; }
    public string? Photo { get; init; }
    public string? Type { get; init; }
    public string? Reason { get; init; }
}