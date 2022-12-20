using ApiFootball.Models.Responses.Inner;
using ApiFootball.Models.Responses.Outer;
using Newtonsoft.Json;

namespace ApiFootball; 

public class FixturesClient : BaseClient, IFixturesClient {
    public FixturesClient(HttpClient httpClient) : base(httpClient) { }

    /// <summary>
    /// Gets the rounds from 
    /// </summary>
    /// <param name="league"></param>
    /// <param name="season"></param>
    /// <param name="current"></param>
    /// <returns></returns>
    /// <exception cref="NullReferenceException"></exception>
    public async Task<BaseResponse<string>> GetRounds(int league, int season, bool current) {
        var queryString = BuildQueryString(("league", league), ("season", season), ("current", current));
        var response = await HttpClient.GetStringAsync(queryString);
        var responseObject =  JsonConvert.DeserializeObject<BaseResponse<string>>(response, SerializerSettings);
        if (responseObject is null) throw new NullReferenceException("Could not deserialize response.");
        return responseObject;
    }
    
    /// <summary>
    /// Gets a list of fixtures.
    /// </summary>
    /// <param name="id">Fixture ID to retrieve</param>
    /// <param name="ids">List of fixture IDs to retrieve</param>
    /// <param name="live"></param>
    /// <param name="date"></param>
    /// <param name="league"></param>
    /// <param name="season"></param>
    /// <param name="team"></param>
    /// <param name="last"></param>
    /// <param name="next"></param>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="round"></param>
    /// <param name="status"></param>
    /// <param name="venue"></param>
    /// <param name="timezone"></param>
    /// <returns></returns>
    /// <exception cref="NullReferenceException">If response cannot be deserialized</exception>
    public async Task<BaseResponse<FixturesResponse>> GetFixtures(int? id = null, IEnumerable<string>? ids = null, string? live = null, DateOnly? date = null, int? league = null, int? season = null, int? team = null, int? last = null, int? next = null, DateOnly? from = null, DateOnly? to = null, string? round = null, string? status = null, int? venue = null, string? timezone = null) {
        var queryString = BuildQueryString((nameof(id), id), (nameof(ids), string.Join("-", ids ?? Array.Empty<string>())), (nameof(live), live), (nameof(date), date), (nameof(league), league), (nameof(season), season), (nameof(team), team), (nameof(last), last), (nameof(next), next), (nameof(from), from), (nameof(to), to), (nameof(round), round), (nameof(status), status), (nameof(venue), venue), (nameof(timezone), timezone));
        var response = await HttpClient.GetStringAsync(queryString);
        var responseObject =  JsonConvert.DeserializeObject<BaseResponse<FixturesResponse>>(response, SerializerSettings);
        if (responseObject is null) throw new NullReferenceException("Could not deserialize response.");
        return responseObject;
    }
}