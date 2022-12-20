using ApiFootball.Clients.Interface;
using ApiFootball.Models.Responses;
using Newtonsoft.Json;

namespace ApiFootball.Clients.Implementation;

public class CountriesClient : BaseClient, ICountriesClient {
    public CountriesClient(IHttpClientFactory factory) : base(factory) { }
    protected override string Route => "countries";

    /// <summary>
    ///     Get the list of available countries for the leagues endpoint.
    /// </summary>
    /// <param name="name">Name of the country</param>
    /// <param name="code">Alpha2 code of the country</param>
    /// <param name="search">Search for the name of a country</param>
    /// <exception cref="NullReferenceException">If unable to deserialize response</exception>
    public async Task<BaseResponse<string>> GetCountries(string? name, string? code, string? search) {
        var queryString = BuildQueryString((nameof(name), name), (nameof(code), code), (nameof(search), search));
        var response = await HttpClient.GetStringAsync(queryString);
        var responseObject = JsonConvert.DeserializeObject<BaseResponse<string>>(response, SerializerSettings);
        if (responseObject is null) throw new NullReferenceException("Could not deserialize response.");
        return responseObject;
    }
}