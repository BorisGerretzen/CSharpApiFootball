using ApiFootball.Options;
using Microsoft.Extensions.Options;

namespace ApiFootball.Clients.Implementation;

public class TimezoneClient(IHttpClientFactory factory, IOptions<ApiFootballOptions> options) : BaseClient(factory, options), ITimezoneClient
{
    protected override string Route => "timezone";

    /// <inheritdoc />
    public async Task<PagedResponse<string>> GetTimezones(CancellationToken cancellationToken = default)
    {
        var queryString = BuildQueryString();
        return await GetAsync<PagedResponse<string>>(queryString, cancellationToken);
    }
}
