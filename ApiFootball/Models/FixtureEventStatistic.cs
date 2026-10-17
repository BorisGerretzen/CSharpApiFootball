namespace ApiFootball.Models;

public class FixtureEventStatistic
{
    public required string Type { get; init; }
    public required JsonElement? Value { get; init; }
}