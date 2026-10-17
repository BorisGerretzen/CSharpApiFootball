namespace ApiFootball.Clients.Interface;

public interface IOddsClient
{
    Task<PagedResponse<OddsResponse>> GetOdds(int? fixture = null, int? league = null, int? season = null,
        DateOnly? date = null, string? timezone = null,
        int? page = null, int? bookmaker = null, int? bet = null, CancellationToken cancellationToken = default);

    Task<PagedResponse<OddsMapping>> GetOddsMapping(int? page = null, CancellationToken cancellationToken = default);

    Task<PagedResponse<OddsLookup>> GetBookmakers(int? id = null, string? search = null,
        CancellationToken cancellationToken = default);

    Task<PagedResponse<OddsLookup>> GetBets(string? id = null, string? search = null,
        CancellationToken cancellationToken = default);

    Task<PagedResponse<LiveOddsResponse>> GetLiveOdds(int? fixture = null, int? league = null, int? bet = null,
        CancellationToken cancellationToken = default);

    Task<PagedResponse<OddsLookup>> GetLiveBets(string? id = null, string? search = null,
        CancellationToken cancellationToken = default);
}