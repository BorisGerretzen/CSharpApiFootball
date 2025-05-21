using Microsoft.Extensions.Options;

namespace ApiFootball.Clients.Implementation;

public class FixturesClient(IHttpClientFactory factory, IOptions<ApiFootballOptions> options) : BaseClient(factory, options), IFixturesClient
{
    protected override string Route => "fixtures";

    /// <inheritdoc />
    public async Task<BaseResponse<string>> GetRounds(int league, int season, bool? current = null, CancellationToken cancellationToken = default)
    {
        var queryString = BuildQueryString("/rounds", ("league", league), ("season", season), ("current", current));
        await using var response = await HttpClient.GetStreamAsync(queryString, cancellationToken);
        return await DeserializeAsync<string>(response, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<BaseResponse<FixturesResponse>> GetFixtures(int? id = null, IEnumerable<string>? ids = null, string? live = null, DateOnly? date = null, int? league = null, int? season = null,
        int? team = null, int? last = null, int? next = null, DateOnly? from = null, DateOnly? to = null, string? round = null, string? status = null, int? venue = null, string? timezone = null,
        CancellationToken cancellationToken = default)
    {
        var queryString = BuildQueryString((nameof(id), id), (nameof(ids), string.Join("-", ids ?? Array.Empty<string>())), (nameof(live), live), (nameof(date), date), (nameof(league), league),
            (nameof(season), season), (nameof(team), team), (nameof(last), last), (nameof(next), next), (nameof(from), from), (nameof(to), to), (nameof(round), round), (nameof(status), status),
            (nameof(venue), venue), (nameof(timezone), timezone));
        await using var response = await HttpClient.GetStreamAsync(queryString, cancellationToken);
        return await DeserializeAsync<FixturesResponse>(response, cancellationToken);
    }
}