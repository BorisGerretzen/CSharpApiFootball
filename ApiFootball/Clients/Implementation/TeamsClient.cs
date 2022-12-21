using ApiFootball.Clients.Interface;
using ApiFootball.Models;
using ApiFootball.Models.Responses;
using ApiFootball.Models.Responses.Inner;
using Newtonsoft.Json;

namespace ApiFootball.Clients.Implementation;

public class TeamsClient : BaseClient, ITeamsClient
{
    public TeamsClient(IHttpClientFactory factory) : base(factory)
    {
    }

    protected override string Route => "teams";

    public async Task<BaseResponse<TeamInformationResponse>> GetTeamsInformation(int? id = null, string? name = null, int? league = null, int? season = null, string? country = null, string? code = null, int? venue = null, string? search = null)
    {
        var queryString = BuildQueryString((nameof(id), id), (nameof(name), name), (nameof(league), league), (nameof(season), season), (nameof(country), country), (nameof(code), code), (nameof(venue), venue), (nameof(search), search));
        var response = await HttpClient.GetStringAsync(queryString);
        var responseObject = JsonConvert.DeserializeObject<BaseResponse<TeamInformationResponse>>(response, SerializerSettings);
        if (responseObject is null) throw new NullReferenceException("Could not deserialize response.");
        return responseObject;
    }

    public async Task<BaseResponse<Season>> GetTeamsSeasons(int team)
    {
        var queryString = BuildQueryString("/seasons", (nameof(team), team));
        var response = await HttpClient.GetStringAsync(queryString);
        var responseObject = JsonConvert.DeserializeObject<BaseResponse<Season>>(response, SerializerSettings);
        if (responseObject is null) throw new NullReferenceException("Could not deserialize response.");
        return responseObject;
    }

    public async Task<BaseResponse<Country>> GetTeamsCountries()
    {
        var queryString = BuildQueryString("/countries");
        var response = await HttpClient.GetStringAsync(queryString);
        var responseObject = JsonConvert.DeserializeObject<BaseResponse<Country>>(response, SerializerSettings);
        if (responseObject is null) throw new NullReferenceException("Could not deserialize response.");
        return responseObject;
    }
}