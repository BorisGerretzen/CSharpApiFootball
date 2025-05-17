namespace ApiFootball.Clients.Interface;

public interface ITeamsClient
{
    /// <summary>
    /// Get the list of available teams.
    /// This endpoint requires at least one parameter.
    /// Recommended Calls: 1 call per day.
    /// </summary>
    /// <param name="id">The id of the team.</param>
    /// <param name="name">The name of the team.</param>
    /// <param name="league">The id of the league.</param>
    /// <param name="season">The season of the league.</param>
    /// <param name="country">The country name of the team.</param>
    /// <param name="code">The code of the team (= 3 characters).</param>
    /// <param name="venue">The id of the venue.</param>
    /// <param name="search">The name or the country of the team (>= 3 characters).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public Task<BaseResponse<TeamInformationResponse>> GetTeamsInformation(int? id = null, string? name = null, int? league = null, int? season = null, string? country = null, string? code = null,
        int? venue = null, string? search = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get the list of seasons available for a team.
    /// Recommended Calls: 1 call per day.
    /// </summary>
    /// <param name="team">The team id.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public Task<BaseResponse<Season>> GetTeamsSeasons(int team, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Get the list of countries available for the teams endpoint.
    /// Recommended Calls: 1 call per day.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    public Task<BaseResponse<Country>> GetTeamsCountries(CancellationToken cancellationToken = default);
}