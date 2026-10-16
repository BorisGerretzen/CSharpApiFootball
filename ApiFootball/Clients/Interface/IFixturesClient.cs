namespace ApiFootball.Clients.Interface;

public interface IFixturesClient
{
    /// <summary>
    /// Get the rounds for a league or a cup.
    /// Recommended Calls: 1 call per day.
    /// </summary>
    /// <param name="league">The ID of the league</param>
    /// <param name="season">The season of the league</param>
    /// <param name="current">Get the current round only</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public Task<PagedResponse<string>> GetRounds(int league, int season, bool? current = null, CancellationToken cancellationToken = default);

    /// <summary>Get league rounds with their fixture dates.</summary>
    public Task<PagedResponse<FixtureRoundDetails>> GetRoundsWithDates(int league, int season, bool? current = null, string? timezone = null, CancellationToken cancellationToken = default);

    /// <summary>Get rounds using the requested timezone without expanding their dates.</summary>
    public Task<PagedResponse<string>> GetRoundsInTimezone(int league, int season, string? timezone, bool? current = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a list of fixtures.
    /// Recommended Calls: 1 call per minute for the leagues, teams, fixtures who have at least one fixture in progress otherwise 1 call per day.
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
    /// <param name="cancellationToken">Cancellation token.</param>
    public Task<PagedResponse<FixturesResponse>> GetFixtures(int? id = null, IEnumerable<string>? ids = null, string? live = null, DateOnly? date = null, int? league = null, int? season = null,
        int? team = null, int? last = null, int? next = null, DateOnly? from = null, DateOnly? to = null, string? round = null, string? status = null, int? venue = null, string? timezone = null,
        CancellationToken cancellationToken = default);

    /// <summary>Get fixtures played by two teams.</summary>
    public Task<PagedResponse<FixturesResponse>> GetHeadToHead(string h2h, DateOnly? date = null, int? league = null, int? season = null, int? last = null,
        int? next = null, DateOnly? from = null, DateOnly? to = null, string? status = null, int? venue = null, string? timezone = null, CancellationToken cancellationToken = default);

    /// <summary>Get the match statistics for a fixture.</summary>
    public Task<PagedResponse<FixtureEventStatistics>> GetFixtureStatistics(int fixture, int? team = null, string? type = null, bool? half = null, CancellationToken cancellationToken = default);

    /// <summary>Get the events for a fixture.</summary>
    public Task<PagedResponse<FixtureEvent>> GetFixtureEvents(int fixture, int? team = null, int? player = null, string? type = null, CancellationToken cancellationToken = default);

    /// <summary>Get the lineups for a fixture.</summary>
    public Task<PagedResponse<FixtureLineup>> GetFixtureLineups(int fixture, int? team = null, int? player = null, string? type = null, CancellationToken cancellationToken = default);

    /// <summary>Get player statistics for a fixture.</summary>
    public Task<PagedResponse<FixturePlayers>> GetFixturePlayers(int fixture, int? team = null, CancellationToken cancellationToken = default);
}
