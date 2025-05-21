using Microsoft.Extensions.Options;

namespace ApiFootball.Clients.Implementation;

public class TimezoneClient(IHttpClientFactory factory, IOptions<ApiFootballOptions> options) : BaseClient(factory, options), ITimezoneClient
{
    protected override string Route => "timezone";

    /// <inheritdoc />
    public async Task<BaseResponse<string>> GetTimezones(CancellationToken cancellationToken = default)
    {
        var queryString = BuildQueryString();
        await using var response = await HttpClient.GetStreamAsync(queryString, cancellationToken);
        return await DeserializeAsync<string>(response, cancellationToken);
    }
}