namespace ApiFootball.Models;

public sealed class LiveOddsTeams
{
    public LiveOddsTeam? Home { get; init; }
    public LiveOddsTeam? Away { get; init; }
}