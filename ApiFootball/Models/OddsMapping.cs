namespace ApiFootball.Models;

public sealed class OddsMapping
{
    public OddsMappingLeague? League { get; init; }
    public OddsMappingFixture? Fixture { get; init; }
    public DateTimeOffset? Update { get; init; }
}