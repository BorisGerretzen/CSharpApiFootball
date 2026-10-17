namespace ApiFootball.Models;

public sealed class CoachCareerEntry
{
    public CoachCareerTeam? Team { get; init; }
    public DateOnly? Start { get; init; }
    public DateOnly? End { get; init; }
}