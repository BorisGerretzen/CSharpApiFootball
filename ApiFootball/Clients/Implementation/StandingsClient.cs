using ApiFootball.Clients.Interface;
using ApiFootball.Models;
using ApiFootball.Models.Responses;
using ApiFootball.Models.Responses.Inner;
using Newtonsoft.Json;

namespace ApiFootball.Clients.Implementation;
public class StandingsClient : BaseClient, IStandingsClient
{
    public StandingsClient(IHttpClientFactory factory) : base(factory) { }
    protected override string Route => "standings";

    /// <summary>
    /// Get the rounds for a league or a cup.
    /// </summary>
    /// <param name="season">The season of the league</param>
    /// <param name="league">The ID of the league</param>
    /// <param name="team">The ID of the team</param>
    /// <exception cref="NullReferenceException">If unable to deserialize response</exception>
    public async Task<BaseResponse<StandingsResponse>> GetStandings(int season, int? league = null, int? team = null)
    {
        var queryString = BuildQueryString("", ("season", season), ("league", league), ("team", team));
        var response = await HttpClient.GetStringAsync(queryString);
        var responseObject = JsonConvert.DeserializeObject<BaseResponse<StandingsResponse>>(response, SerializerSettings);
        if (responseObject is null) throw new NullReferenceException("Could not deserialize response.");
        return responseObject;
    }
}