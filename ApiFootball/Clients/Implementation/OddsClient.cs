using ApiFootball.Options;
using Microsoft.Extensions.Options;

namespace ApiFootball.Clients.Implementation;

public sealed class OddsClient(IHttpClientFactory factory, IOptions<ApiFootballOptions> options) : BaseClient(factory, options), IOddsClient
{
    protected override string Route => "odds";

    public async Task<PagedResponse<OddsResponse>> GetOdds(int? fixture = null, int? league = null, int? season = null, DateOnly? date = null, string? timezone = null,
        int? page = null, int? bookmaker = null, int? bet = null, CancellationToken cancellationToken = default)
    {
        var query = BuildQueryString((nameof(fixture), fixture), (nameof(league), league), (nameof(season), season), (nameof(date), date), (nameof(timezone), timezone),
            (nameof(page), page), (nameof(bookmaker), bookmaker), (nameof(bet), bet));
        return await GetAsync<PagedResponse<OddsResponse>>(query, cancellationToken);
    }

    public async Task<PagedResponse<OddsMapping>> GetOddsMapping(int? page = null, CancellationToken cancellationToken = default)
    {
        var query = BuildQueryString("/mapping", (nameof(page), page));
        return await GetAsync<PagedResponse<OddsMapping>>(query, cancellationToken);
    }

    public async Task<PagedResponse<OddsLookup>> GetBookmakers(int? id = null, string? search = null, CancellationToken cancellationToken = default)
    {
        var query = BuildQueryString("/bookmakers", (nameof(id), id), (nameof(search), search));
        return await GetAsync<PagedResponse<OddsLookup>>(query, cancellationToken);
    }

    public async Task<PagedResponse<OddsLookup>> GetBets(string? id = null, string? search = null, CancellationToken cancellationToken = default)
    {
        var query = BuildQueryString("/bets", (nameof(id), id), (nameof(search), search));
        return await GetAsync<PagedResponse<OddsLookup>>(query, cancellationToken);
    }

    public async Task<PagedResponse<LiveOddsResponse>> GetLiveOdds(int? fixture = null, int? league = null, int? bet = null,
        CancellationToken cancellationToken = default)
    {
        var query = BuildQueryString("/live", (nameof(fixture), fixture), (nameof(league), league), (nameof(bet), bet));
        return await GetAsync<PagedResponse<LiveOddsResponse>>(query, cancellationToken);
    }

    public async Task<PagedResponse<OddsLookup>> GetLiveBets(string? id = null, string? search = null, CancellationToken cancellationToken = default)
    {
        var query = BuildQueryString("/live/bets", (nameof(id), id), (nameof(search), search));
        return await GetAsync<PagedResponse<OddsLookup>>(query, cancellationToken);
    }
}
