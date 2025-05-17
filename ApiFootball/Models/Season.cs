namespace ApiFootball.Models;

public class Season
{
    public int Year { get; init; }
    public DateOnly Start { get; init; }
    public DateOnly End { get; init; }
    public bool Current { get; init; }
    public required Coverage Coverage { get; init; }
}