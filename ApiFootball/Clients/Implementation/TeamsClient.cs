using Microsoft.Extensions.Options;

namespace ApiFootball.Clients.Implementation;

public class TeamsClient(IHttpClientFactory factory, IOptions<ApiFootballOptions> options) : BaseClient(factory, options), ITeamsClient
{
    protected override string Route => "teams";

    /// <inheritdoc />
    public async Task<BaseResponse<TeamInformationResponse>> GetTeamsInformation(int? id = null, string? name = null, int? league = null, int? season = null, string? country = null,
        string? code = null, int? venue = null, string? search = null, CancellationToken cancellationToken = default)
    {
        var queryString = BuildQueryString((nameof(id), id), (nameof(name), name), (nameof(league), league), (nameof(season), season), (nameof(country), country), (nameof(code), code),
            (nameof(venue), venue), (nameof(search), search));
        await using var response = await HttpClient.GetStreamAsync(queryString, cancellationToken);
        return await DeserializeAsync<TeamInformationResponse>(response, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<BaseResponse<int>> GetTeamsSeasons(int team, CancellationToken cancellationToken = default)
    {
        var queryString = BuildQueryString("/seasons", (nameof(team), team));
        await using var response = await HttpClient.GetStreamAsync(queryString, cancellationToken);
        return await DeserializeAsync<int>(response, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<BaseResponse<Country>> GetTeamsCountries(CancellationToken cancellationToken = default)
    {
        var queryString = BuildQueryString("/countries");
        await using var response = await HttpClient.GetStreamAsync(queryString, cancellationToken);
        return await DeserializeAsync<Country>(response, cancellationToken);
    }
}