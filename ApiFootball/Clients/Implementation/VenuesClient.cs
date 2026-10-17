using ApiFootball.Options;
using Microsoft.Extensions.Options;

namespace ApiFootball.Clients.Implementation;

public class VenuesClient(IHttpClientFactory factory, IOptions<ApiFootballOptions> options)
    : BaseClient(factory, options), IVenuesClient
{
    protected override string Route => "venues";

    public async Task<PagedResponse<Venue>> GetVenues(int? id = null, string? name = null, string? city = null,
        string? country = null, string? search = null,
        CancellationToken cancellationToken = default)
    {
        var queryString = BuildQueryString((nameof(id), id), (nameof(name), name), (nameof(city), city),
            (nameof(country), country), (nameof(search), search));
        return await GetAsync<PagedResponse<Venue>>(queryString, cancellationToken);
    }
}