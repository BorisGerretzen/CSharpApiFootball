namespace ApiFootball.Models;

public sealed class CoachProfile
{
    public int Id { get; init; }
    public string? Name { get; init; }
    public string? Firstname { get; init; }
    public string? Lastname { get; init; }
    public int? Age { get; init; }
    public PlayerBirth? Birth { get; init; }
    public string? Nationality { get; init; }
    public string? Height { get; init; }
    public string? Weight { get; init; }
    public string? Photo { get; init; }
    public TeamReference? Team { get; init; }
    public List<CoachCareerEntry>? Career { get; init; }
}