namespace ApiFootball.Clients.Interface;

public interface ICountriesClient
{
    /// <summary>
    /// Get the list of available countries for the leagues endpoint.
    /// Recommended Calls: 1 call per day.
    /// </summary>
    /// <param name="name">Name of the country</param>
    /// <param name="code">Alpha2 code of the country</param>
    /// <param name="search">Search for the name of a country</param>
    /// <param name="cancellationToken"></param>
    public Task<BaseResponse<Country>> GetCountries(string? name = null, string? code = null, string? search = null, CancellationToken cancellationToken = default);
}