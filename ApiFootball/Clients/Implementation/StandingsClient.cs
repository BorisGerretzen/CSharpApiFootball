namespace ApiFootball.Clients.Implementation;

public class StandingsClient(IHttpClientFactory factory) : BaseClient(factory), IStandingsClient
{
    protected override string Route => "standings";

    /// <inheritdoc />
    public async Task<BaseResponse<StandingsResponse>> GetStandings(int season, int? league = null, int? team = null, CancellationToken cancellationToken = default)
    {
        var queryString = BuildQueryString("", ("season", season), ("league", league), ("team", team));
        await using var response = await HttpClient.GetStreamAsync(queryString, cancellationToken);
        return await JsonSerializer.DeserializeAsync<BaseResponse<StandingsResponse>>(response, SerializerOptions, cancellationToken)
               ?? throw new NullReferenceException("Could not deserialize response.");
    }
}