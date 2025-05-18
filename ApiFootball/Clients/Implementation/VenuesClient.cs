namespace ApiFootball.Clients.Implementation;

public class VenuesClient(IHttpClientFactory factory) : BaseClient(factory), IVenuesClient
{
    protected override string Route => "venues";

    public async Task<BaseResponse<Venue>> GetVenues(int? id = null, string? name = null, string? city = null, string? country = null, string? search = null,
        CancellationToken cancellationToken = default)
    {
        var queryString = BuildQueryString((nameof(id), id), (nameof(name), name), (nameof(city), city), (nameof(country), country), (nameof(search), search));
        await using var response = await HttpClient.GetStreamAsync(queryString, cancellationToken);
        return await JsonSerializer.DeserializeAsync<BaseResponse<Venue>>(response, SerializerOptions, cancellationToken)
               ?? throw new NullReferenceException("Could not deserialize response.");
    }
}