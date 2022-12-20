using ApiFootball.Clients.Interface;
using ApiFootball.Models;
using ApiFootball.Models.Responses;
using ApiFootball.Models.Responses.Inner;
using Newtonsoft.Json;

namespace ApiFootball.Clients.Implementation;

public class LeaguesClient : BaseClient, ILeaguesClient {
    public LeaguesClient(IHttpClientFactory factory) : base(factory) { }
    protected override string Route => "leagues";

    /// <summary>
    ///     Get the list of available leagues and cups.
    /// </summary>
    /// <param name="id">The id of the league</param>
    /// <param name="name">The name of the league</param>
    /// <param name="country">The country name of the league</param>
    /// <param name="code">The Alpha2 code of the country</param>
    /// <param name="season">The season of the league</param>
    /// <param name="team">The id of the team</param>
    /// <param name="type">The type of the league</param>
    /// <param name="current">The state of the league</param>
    /// <param name="search">The name or the country of the league</param>
    /// <param name="last">The X last leagues/cups added in the API</param>
    /// <exception cref="NullReferenceException">If unable to deserialize response</exception>
    public async Task<BaseResponse<LeaguesResponse>> GetLeagues(int? id, string? name, string? country, string? code, int? season, int? team, LeagueType? type, bool? current, string? search,
        int? last) {
        var queryString = BuildQueryString((nameof(id), id), (nameof(name), name), (nameof(country), country), (nameof(code), code), (nameof(season), season), (nameof(team), team),
            (nameof(type), type), (nameof(current), current), (nameof(search), search), (nameof(last), last));
        var response = await HttpClient.GetStringAsync(queryString);
        var responseObject = JsonConvert.DeserializeObject<BaseResponse<LeaguesResponse>>(response, SerializerSettings);
        if (responseObject is null) throw new NullReferenceException("Could not deserialize response.");
        return responseObject;
    }

    /// <summary>
    ///     Get the list of available seasons.
    /// </summary>
    /// <exception cref="NullReferenceException">If unable to deserialize response</exception>
    public async Task<BaseResponse<int>> GetSeasons() {
        var queryString = BuildQueryString();
        var response = await HttpClient.GetStringAsync("/seasons" + queryString);
        var responseObject = JsonConvert.DeserializeObject<BaseResponse<int>>(response, SerializerSettings);
        if (responseObject is null) throw new NullReferenceException("Could not deserialize response.");
        return responseObject;
    }
}