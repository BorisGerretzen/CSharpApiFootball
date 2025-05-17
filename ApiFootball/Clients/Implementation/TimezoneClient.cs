namespace ApiFootball.Clients.Implementation;

public class TimezoneClient(IHttpClientFactory factory) : BaseClient(factory), ITimezoneClient
{
    protected override string Route => "timezone";

    /// <inheritdoc />
    public async Task<BaseResponse<string>> GetTimezones(CancellationToken cancellationToken = default)
    {
        var queryString = BuildQueryString();
        await using var response = await HttpClient.GetStreamAsync(queryString, cancellationToken);
        return await JsonSerializer.DeserializeAsync<BaseResponse<string>>(response, SerializerOptions, cancellationToken)
               ?? throw new NullReferenceException("Could not deserialize response.");
    }
}