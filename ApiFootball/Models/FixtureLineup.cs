namespace ApiFootball.Models;

public class FixtureLineup
{
    public required FixtureLineupTeam Team { get; init; }
    public required Coach Coach { get; init; }
    public required string Formation { get; init; }
    public required List<FixtureLineupPlayer> StartXI { get; init; }
    public required List<FixtureLineupPlayer> Substitutes { get; init; }
}
