namespace ApiFootball.Models;

public sealed class OddsResponse
{
    public OddsLeague? League { get; init; }
    public FixtureReference? Fixture { get; init; }
    public DateTimeOffset? Update { get; init; }
    public List<OddsBookmaker>? Bookmakers { get; init; }
}