namespace ApiFootball.Models;

public sealed class Score
{
    public required Goals Halftime { get; init; }
    public required Goals Fulltime { get; init; }
    public required Goals Extratime { get; init; }
    public required Goals Penalty { get; init; }
}