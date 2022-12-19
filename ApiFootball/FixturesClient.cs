using ApiFootball.Models.Response;
using Newtonsoft.Json;

namespace ApiFootball; 

public class FixturesClient : BaseClient {
    public FixturesClient(HttpClient httpClient) : base(httpClient) { }

    public async Task<StringResponses> GetRounds(int league, int season, bool current) {
        var queryString = BuildQueryString(("league", league), ("season", season), ("current", current));
        var response = await HttpClient.GetStringAsync(queryString);
        var responseObject =  JsonConvert.DeserializeObject<StringResponses>(response);
        if (responseObject is null) throw new NullReferenceException("Could not deserialize response.");
        return responseObject;
    }
    
    public async Task<FixturesResponses> GetFixtures(int? id = null, IEnumerable<string>? ids = null, string? live = null, DateOnly? date = null, int? league = null, int? season = null, int? team = null, int? last = null, int? next = null, DateOnly? from = null, DateOnly? to = null, string? round = null, string? status = null, int? venue = null, string? timezone = null) {
        var queryString = BuildQueryString((nameof(id), id), (nameof(ids), string.Join("-", ids ?? Array.Empty<string>())), (nameof(live), live), (nameof(date), date), (nameof(league), league), (nameof(season), season), (nameof(team), team), (nameof(last), last), (nameof(next), next), (nameof(from), from), (nameof(to), to), (nameof(round), round), (nameof(status), status), (nameof(venue), venue), (nameof(timezone), timezone));
        var response = await HttpClient.GetStringAsync(queryString);
        var responseObject =  JsonConvert.DeserializeObject<FixturesResponses>(response, SerializerSettings);
        if (responseObject is null) throw new NullReferenceException("Could not deserialize response.");
        return responseObject;
    }
}