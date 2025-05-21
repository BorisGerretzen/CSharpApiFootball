using Microsoft.Extensions.Options;

namespace ApiFootball.Clients.Implementation;

public class CountriesClient(IHttpClientFactory factory, IOptions<ApiFootballOptions> options) : BaseClient(factory, options), ICountriesClient
{
    protected override string Route => "countries";

    /// <inheritdoc />
    public async Task<BaseResponse<Country>> GetCountries(string? name = null, string? code = null, string? search = null, CancellationToken cancellationToken = default)
    {
        var queryString = BuildQueryString((nameof(name), name), (nameof(code), code), (nameof(search), search));
        await using var response = await HttpClient.GetStreamAsync(queryString, cancellationToken);
        return await DeserializeAsync<Country>(response, cancellationToken);
    }
}