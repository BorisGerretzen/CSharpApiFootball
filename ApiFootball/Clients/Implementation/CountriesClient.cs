namespace ApiFootball.Clients.Implementation;

public class CountriesClient(IHttpClientFactory factory) : BaseClient(factory), ICountriesClient
{
    protected override string Route => "countries";

    /// <inheritdoc />
    public async Task<BaseResponse<Country>> GetCountries(string? name = null, string? code = null, string? search = null, CancellationToken cancellationToken = default)
    {
        var queryString = BuildQueryString((nameof(name), name), (nameof(code), code), (nameof(search), search));
        await using var response = await HttpClient.GetStreamAsync(queryString, cancellationToken);
        return await JsonSerializer.DeserializeAsync<BaseResponse<Country>>(response, SerializerOptions, cancellationToken)
               ?? throw new NullReferenceException("Could not deserialize response.");
    }
}