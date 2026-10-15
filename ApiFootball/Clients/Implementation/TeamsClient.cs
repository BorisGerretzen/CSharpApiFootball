using ApiFootball.Options;
using Microsoft.Extensions.Options;

namespace ApiFootball.Clients.Implementation;

public class TeamsClient(IHttpClientFactory factory, IOptions<ApiFootballOptions> options) : BaseClient(factory, options), ITeamsClient
{
    protected override string Route => "teams";

    /// <inheritdoc />
    public async Task<PagedResponse<TeamInformationResponse>> GetTeamsInformation(int? id = null, string? name = null, int? league = null, int? season = null, string? country = null,
        string? code = null, int? venue = null, string? search = null, CancellationToken cancellationToken = default)
    {
        var queryString = BuildQueryString((nameof(id), id), (nameof(name), name), (nameof(league), league), (nameof(season), season), (nameof(country), country), (nameof(code), code),
            (nameof(venue), venue), (nameof(search), search));
        return await GetAsync<PagedResponse<TeamInformationResponse>>(queryString, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<PagedResponse<int>> GetTeamsSeasons(int team, CancellationToken cancellationToken = default)
    {
        var queryString = BuildQueryString("/seasons", (nameof(team), team));
        return await GetAsync<PagedResponse<int>>(queryString, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<PagedResponse<Country>> GetTeamsCountries(CancellationToken cancellationToken = default)
    {
        var queryString = BuildQueryString("/countries");
        return await GetAsync<PagedResponse<Country>>(queryString, cancellationToken);
    }
}
