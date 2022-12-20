using ApiFootball.Models.Responses.Inner;
using ApiFootball.Models.Responses.Outer;
using Newtonsoft.Json;

namespace ApiFootball;

public class FixturesClient : BaseClient, IFixturesClient {
    public FixturesClient(IHttpClientFactory factory) : base(factory) { }
    protected override string Route => "fixtures";

    /// <summary>
    /// Get the rounds for a league or a cup.
    /// </summary>
    /// <param name="league">The ID of the league</param>
    /// <param name="season">The season of the league</param>
    /// <param name="current">Get the current round only</param>
    /// <exception cref="NullReferenceException">If unable to deserialize response</exception>
    public async Task<BaseResponse<string>> GetRounds(int league, int season, bool? current) {
        var queryString = BuildQueryString(("league", league), ("season", season), ("current", current));
        var response = await HttpClient.GetStringAsync(queryString);
        var responseObject = JsonConvert.DeserializeObject<BaseResponse<string>>(response, SerializerSettings);
        if (responseObject is null) throw new NullReferenceException("Could not deserialize response.");
        return responseObject;
    }

    /// <summary>
    /// Gets a list of fixtures.
    /// </summary>
    /// <param name="id">Fixture ID to retrieve</param>
    /// <param name="ids">List of fixture IDs to retrieve</param>
    /// <param name="live">All or several leagues ids</param>
    /// <param name="date">A valid date</param>
    /// <param name="league">The id of the league</param>
    /// <param name="season">The season of the league</param>
    /// <param name="team">The id of the team</param>
    /// <param name="last">For the X last fixtures</param>
    /// <param name="next">For the X next fixtures</param>
    /// <param name="from">A valid date</param>
    /// <param name="to">A valid date</param>
    /// <param name="round">The round of the fixture</param>
    /// <param name="status">One or more fixture status short</param>
    /// <param name="venue">The venue id of the fixture</param>
    /// <param name="timezone">A valid timezone from <see cref="ITimezoneClient.GetTimezones"/></param>
    /// <exception cref="NullReferenceException">If response cannot be deserialized</exception>
    public async Task<BaseResponse<FixturesResponse>> GetFixtures(int? id = null, IEnumerable<string>? ids = null, string? live = null, DateOnly? date = null, int? league = null, int? season = null,
        int? team = null, int? last = null, int? next = null, DateOnly? from = null, DateOnly? to = null, string? round = null, string? status = null, int? venue = null, string? timezone = null) {
        var queryString = BuildQueryString((nameof(id), id), (nameof(ids), string.Join("-", ids ?? Array.Empty<string>())), (nameof(live), live), (nameof(date), date), (nameof(league), league),
            (nameof(season), season), (nameof(team), team), (nameof(last), last), (nameof(next), next), (nameof(from), from), (nameof(to), to), (nameof(round), round), (nameof(status), status),
            (nameof(venue), venue), (nameof(timezone), timezone));
        var response = await HttpClient.GetStringAsync(queryString);
        var responseObject = JsonConvert.DeserializeObject<BaseResponse<FixturesResponse>>(response, SerializerSettings);
        if (responseObject is null) throw new NullReferenceException("Could not deserialize response.");
        return responseObject;
    }
}