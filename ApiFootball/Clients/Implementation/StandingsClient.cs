using ApiFootball.Options;
using Microsoft.Extensions.Options;

namespace ApiFootball.Clients.Implementation;

public class StandingsClient(IHttpClientFactory factory, IOptions<ApiFootballOptions> options) : BaseClient(factory, options), IStandingsClient
{
    protected override string Route => "standings";

    /// <inheritdoc />
    public async Task<BaseResponse<StandingsResponse>> GetStandings(int season, int? league = null, int? team = null, CancellationToken cancellationToken = default)
    {
        var queryString = BuildQueryString("", ("season", season), ("league", league), ("team", team));
        return await GetAsync<StandingsResponse>(queryString, cancellationToken);
    }
}
