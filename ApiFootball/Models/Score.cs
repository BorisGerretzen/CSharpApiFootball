namespace ApiFootball.Models;

public class Score
{
    public required Goals Halftime { get; init; }
    public required Goals Fulltime { get; init; }
    public required Goals Extratime { get; init; }
    public required Goals Penalty { get; init; }
}