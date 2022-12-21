using ApiFootball.Clients.Interface;
using ApiFootball.Models;
using ApiFootball.Models.Responses;
using Newtonsoft.Json;

namespace ApiFootball.Clients.Implementation;

public class VenuesClient : BaseClient, IVenuesClient
{
    public VenuesClient(IHttpClientFactory factory) : base(factory)
    {
    }

    protected override string Route { get; }
    public async Task<BaseResponse<Venue>> GetVenues(int? id, string? name, string? city, string? country, string? search)
    {
        var queryString = BuildQueryString((nameof(id), id), (nameof(name), name), (nameof(city), city), (nameof(country), country), (nameof(search), search));
        var response = await HttpClient.GetStringAsync(queryString);
        var responseObject = JsonConvert.DeserializeObject<BaseResponse<Venue>>(response, SerializerSettings);
        if (responseObject is null) throw new NullReferenceException("Could not deserialize response.");
        return responseObject;
    }
}