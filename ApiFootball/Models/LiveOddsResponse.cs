namespace ApiFootball.Models;

public sealed class LiveOddsResponse
{
    public LiveOddsFixture? Fixture { get; init; }
    public LiveOddsLeague? League { get; init; }
    public LiveOddsTeams? Teams { get; init; }
    public LiveOddsStatus? Status { get; init; }
    public DateTimeOffset? Update { get; init; }
    public List<OddsBet>? Odds { get; init; }
}