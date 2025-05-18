namespace ApiFootball.Clients.Interface;

public interface IStandingsClient
{
    /// <summary>
    /// Get the rounds for a league or a cup.
    /// Recommended Calls: 1 call per hour for the leagues or teams who have at least one fixture in progress otherwise 1 call per day.
    /// </summary>
    /// <param name="season">The season of the league</param>
    /// <param name="league">The ID of the league</param>
    /// <param name="team">The ID of the team</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<BaseResponse<StandingsResponse>> GetStandings(
        int season,
        int? league = null,
        int? team = null,
        CancellationToken cancellationToken = default
    );
}