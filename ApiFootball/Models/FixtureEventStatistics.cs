namespace ApiFootball.Models;

public class FixtureEventStatistics
{
    public required TeamReference Team { get; init; }
    public required List<FixtureEventStatistic> Statistics { get; init; }
}