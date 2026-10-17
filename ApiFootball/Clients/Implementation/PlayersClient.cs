using ApiFootball.Options;
using Microsoft.Extensions.Options;

namespace ApiFootball.Clients.Implementation;

public sealed class PlayersClient(IHttpClientFactory factory, IOptions<ApiFootballOptions> options)
    : BaseClient(factory, options), IPlayersClient
{
    protected override string Route => "players";

    public async Task<PagedResponse<int>> GetSeasons(int? player = null, CancellationToken cancellationToken = default)
    {
        var query = BuildQueryString("/seasons", (nameof(player), player));
        return await GetAsync<PagedResponse<int>>(query, cancellationToken);
    }

    public async Task<PagedResponse<PlayerProfileResponse>> GetProfiles(int? player = null, string? search = null,
        int? page = null,
        CancellationToken cancellationToken = default)
    {
        var query = BuildQueryString("/profiles", (nameof(player), player), (nameof(search), search),
            (nameof(page), page));
        return await GetAsync<PagedResponse<PlayerProfileResponse>>(query, cancellationToken);
    }

    public async Task<PagedResponse<PlayerStatisticsResponse>> GetPlayerStatistics(int? id = null, int? team = null,
        int? league = null, int? season = null,
        string? search = null, int? page = null, CancellationToken cancellationToken = default)
    {
        var query = BuildQueryString((nameof(id), id), (nameof(team), team), (nameof(league), league),
            (nameof(season), season), (nameof(search), search), (nameof(page), page));
        return await GetAsync<PagedResponse<PlayerStatisticsResponse>>(query, cancellationToken);
    }

    public async Task<PagedResponse<PlayerSquadResponse>> GetSquads(int? team = null, int? player = null,
        CancellationToken cancellationToken = default)
    {
        var query = BuildQueryString("/squads", (nameof(team), team), (nameof(player), player));
        return await GetAsync<PagedResponse<PlayerSquadResponse>>(query, cancellationToken);
    }

    public async Task<PagedResponse<PlayerTeamHistory>> GetTeams(int player,
        CancellationToken cancellationToken = default)
    {
        var query = BuildQueryString("/teams", (nameof(player), player));
        return await GetAsync<PagedResponse<PlayerTeamHistory>>(query, cancellationToken);
    }

    public Task<PagedResponse<PlayerStatisticsResponse>> GetTopScorers(int league, int season,
        CancellationToken cancellationToken = default)
    {
        return GetLeaders("/topscorers", league, season, cancellationToken);
    }

    public Task<PagedResponse<PlayerStatisticsResponse>> GetTopAssists(int league, int season,
        CancellationToken cancellationToken = default)
    {
        return GetLeaders("/topassists", league, season, cancellationToken);
    }

    public Task<PagedResponse<PlayerStatisticsResponse>> GetTopYellowCards(int league, int season,
        CancellationToken cancellationToken = default)
    {
        return GetLeaders("/topyellowcards", league, season, cancellationToken);
    }

    public Task<PagedResponse<PlayerStatisticsResponse>> GetTopRedCards(int league, int season,
        CancellationToken cancellationToken = default)
    {
        return GetLeaders("/topredcards", league, season, cancellationToken);
    }

    private Task<PagedResponse<PlayerStatisticsResponse>> GetLeaders(string path, int league, int season,
        CancellationToken cancellationToken)
    {
        var query = BuildQueryString(path, (nameof(league), league), (nameof(season), season));
        return GetAsync<PagedResponse<PlayerStatisticsResponse>>(query, cancellationToken);
    }
}