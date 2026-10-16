namespace ApiFootball.Models;

public sealed class PlayerTeamHistory
{
    public TeamReference? Team { get; init; }
    public List<int>? Seasons { get; init; }
}