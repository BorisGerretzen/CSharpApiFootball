namespace ApiFootball.Models;

public sealed class InjuryResponse
{
    public InjuryPlayer? Player { get; init; }
    public TeamReference? Team { get; init; }
    public FixtureReference? Fixture { get; init; }
    public TeamStatisticsLeague? League { get; init; }
}