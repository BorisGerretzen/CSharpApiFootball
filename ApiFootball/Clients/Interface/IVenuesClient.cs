namespace ApiFootball.Clients.Interface;

public interface IVenuesClient
{
    /// <summary>
    /// Get the list of available venues.
    /// This endpoint requires at least one parameter.
    /// Recommended Calls: 1 call per day.
    /// </summary>
    /// <param name="id">The id of the venue.</param>
    /// <param name="name">The name of the venue.</param>
    /// <param name="city">The city of the venue.</param>
    /// <param name="country">The country of the venue.</param>
    /// <param name="search">The name, city or the country of the venue (>= 3 characters).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public Task<BaseResponse<Venue>> GetVenues(int? id = null, string? name = null, string? city = null, string? country = null, string? search = null, CancellationToken cancellationToken = default);
}