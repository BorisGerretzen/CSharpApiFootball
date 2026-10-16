namespace ApiFootball.Models;

public sealed class PlayerSquadResponse
{
    public TeamReference? Team { get; init; }
    public List<PlayerSquadMember>? Players { get; init; }
}