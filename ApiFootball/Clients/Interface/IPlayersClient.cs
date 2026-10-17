namespace ApiFootball.Clients.Interface;

public interface IPlayersClient
{
    Task<PagedResponse<int>> GetSeasons(int? player = null, CancellationToken cancellationToken = default);

    Task<PagedResponse<PlayerProfileResponse>> GetProfiles(int? player = null, string? search = null, int? page = null,
        CancellationToken cancellationToken = default);

    Task<PagedResponse<PlayerStatisticsResponse>> GetPlayerStatistics(int? id = null, int? team = null,
        int? league = null, int? season = null,
        string? search = null, int? page = null, CancellationToken cancellationToken = default);

    Task<PagedResponse<PlayerSquadResponse>> GetSquads(int? team = null, int? player = null,
        CancellationToken cancellationToken = default);

    Task<PagedResponse<PlayerTeamHistory>> GetTeams(int player, CancellationToken cancellationToken = default);

    Task<PagedResponse<PlayerStatisticsResponse>> GetTopScorers(int league, int season,
        CancellationToken cancellationToken = default);

    Task<PagedResponse<PlayerStatisticsResponse>> GetTopAssists(int league, int season,
        CancellationToken cancellationToken = default);

    Task<PagedResponse<PlayerStatisticsResponse>> GetTopYellowCards(int league, int season,
        CancellationToken cancellationToken = default);

    Task<PagedResponse<PlayerStatisticsResponse>> GetTopRedCards(int league, int season,
        CancellationToken cancellationToken = default);
}