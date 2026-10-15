using ApiFootball.Options;
using Microsoft.Extensions.Options;

namespace ApiFootball.Clients.Implementation;

public class LeaguesClient(IHttpClientFactory factory, IOptions<ApiFootballOptions> options) : BaseClient(factory, options), ILeaguesClient
{
    protected override string Route => "leagues";

    /// <inheritdoc />
    public async Task<PagedResponse<LeaguesResponse>> GetLeagues(int? id = null, string? name = null, string? country = null, string? code = null, int? season = null, int? team = null,
        LeagueType? type = null, bool? current = null, string? search = null,
        int? last = null, CancellationToken cancellationToken = default)
    {
        var queryString = BuildQueryString((nameof(id), id), (nameof(name), name), (nameof(country), country), (nameof(code), code), (nameof(season), season), (nameof(team), team),
            (nameof(type), type), (nameof(current), current), (nameof(search), search), (nameof(last), last));
        return await GetAsync<PagedResponse<LeaguesResponse>>(queryString, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<PagedResponse<int>> GetSeasons(CancellationToken cancellationToken = default)
    {
        var queryString = BuildQueryString("/seasons");
        return await GetAsync<PagedResponse<int>>(queryString, cancellationToken);
    }
}
