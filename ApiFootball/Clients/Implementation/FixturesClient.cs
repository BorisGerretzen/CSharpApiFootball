using ApiFootball.Options;
using Microsoft.Extensions.Options;

namespace ApiFootball.Clients.Implementation;

public class FixturesClient(IHttpClientFactory factory, IOptions<ApiFootballOptions> options) : BaseClient(factory, options), IFixturesClient
{
    protected override string Route => "fixtures";

    /// <inheritdoc />
    public async Task<PagedResponse<string>> GetRounds(int league, int season, bool? current = null, CancellationToken cancellationToken = default)
    {
        var queryString = BuildQueryString("/rounds", ("league", league), ("season", season), ("current", current));
        return await GetAsync<PagedResponse<string>>(queryString, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<PagedResponse<FixtureRoundDetails>> GetRoundsWithDates(int league, int season, bool? current = null, string? timezone = null,
        CancellationToken cancellationToken = default)
    {
        var queryString = BuildQueryString("/rounds", ("league", league), ("season", season), ("current", current), ("dates", true), ("timezone", timezone));
        return await GetAsync<PagedResponse<FixtureRoundDetails>>(queryString, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<PagedResponse<string>> GetRoundsInTimezone(int league, int season, string? timezone, bool? current = null,
        CancellationToken cancellationToken = default)
    {
        var queryString = BuildQueryString("/rounds", ("league", league), ("season", season), ("current", current), ("timezone", timezone));
        return await GetAsync<PagedResponse<string>>(queryString, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<PagedResponse<FixturesResponse>> GetFixtures(int? id = null, IEnumerable<string>? ids = null, string? live = null, DateOnly? date = null, int? league = null, int? season = null,
        int? team = null, int? last = null, int? next = null, DateOnly? from = null, DateOnly? to = null, string? round = null, string? status = null, int? venue = null, string? timezone = null,
        CancellationToken cancellationToken = default)
    {
        var queryString = BuildQueryString((nameof(id), id), (nameof(ids), string.Join("-", ids ?? Array.Empty<string>())), (nameof(live), live), (nameof(date), date), (nameof(league), league),
            (nameof(season), season), (nameof(team), team), (nameof(last), last), (nameof(next), next), (nameof(from), from), (nameof(to), to), (nameof(round), round), (nameof(status), status),
            (nameof(venue), venue), (nameof(timezone), timezone));
        return await GetAsync<PagedResponse<FixturesResponse>>(queryString, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<PagedResponse<FixturesResponse>> GetHeadToHead(string h2h, DateOnly? date = null, int? league = null, int? season = null, int? last = null,
        int? next = null, DateOnly? from = null, DateOnly? to = null, string? status = null, int? venue = null, string? timezone = null, CancellationToken cancellationToken = default)
    {
        var queryString = BuildQueryString("/headtohead", (nameof(h2h), h2h), (nameof(date), date), (nameof(league), league), (nameof(season), season), (nameof(last), last),
            (nameof(next), next), (nameof(from), from), (nameof(to), to), (nameof(status), status), (nameof(venue), venue), (nameof(timezone), timezone));
        return await GetAsync<PagedResponse<FixturesResponse>>(queryString, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<PagedResponse<FixtureEventStatistics>> GetFixtureStatistics(int fixture, int? team = null, string? type = null, bool? half = null,
        CancellationToken cancellationToken = default)
    {
        var queryString = BuildQueryString("/statistics", (nameof(fixture), fixture), (nameof(team), team), (nameof(type), type), (nameof(half), half));
        return await GetAsync<PagedResponse<FixtureEventStatistics>>(queryString, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<PagedResponse<FixtureEvent>> GetFixtureEvents(int fixture, int? team = null, int? player = null, string? type = null,
        CancellationToken cancellationToken = default)
    {
        var queryString = BuildQueryString("/events", (nameof(fixture), fixture), (nameof(team), team), (nameof(player), player), (nameof(type), type));
        return await GetAsync<PagedResponse<FixtureEvent>>(queryString, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<PagedResponse<FixtureLineup>> GetFixtureLineups(int fixture, int? team = null, int? player = null, string? type = null,
        CancellationToken cancellationToken = default)
    {
        var queryString = BuildQueryString("/lineups", (nameof(fixture), fixture), (nameof(team), team), (nameof(player), player), (nameof(type), type));
        return await GetAsync<PagedResponse<FixtureLineup>>(queryString, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<PagedResponse<FixturePlayers>> GetFixturePlayers(int fixture, int? team = null, CancellationToken cancellationToken = default)
    {
        var queryString = BuildQueryString("/players", (nameof(fixture), fixture), (nameof(team), team));
        return await GetAsync<PagedResponse<FixturePlayers>>(queryString, cancellationToken);
    }
}
