namespace ApiFootball.Clients.Interface;

public interface ILeaguesClient
{
    /// <summary>
    /// Get the list of available leagues and cups.
    /// Recommended Calls: 1 call per hour.
    /// </summary>
    /// <param name="id">The id of the league</param>
    /// <param name="name">The name of the league</param>
    /// <param name="country">The country name of the league</param>
    /// <param name="code">The Alpha2 code of the country</param>
    /// <param name="season">The season of the league</param>
    /// <param name="team">The id of the team</param>
    /// <param name="type">The type of the league</param>
    /// <param name="current">The state of the league</param>
    /// <param name="search">The name or the country of the league</param>
    /// <param name="last">The X last leagues/cups added in the API</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public Task<BaseResponse<LeaguesResponse>> GetLeagues(int? id = null, string? name = null, string? country = null, string? code = null, int? season = null, int? team = null,
        LeagueType? type = null, bool? current = null, string? search = null, int? last = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get the list of available seasons.
    /// Recommended Calls: 1 call per day.
    /// </summary>
    public Task<BaseResponse<int>> GetSeasons(CancellationToken cancellationToken = default);
}