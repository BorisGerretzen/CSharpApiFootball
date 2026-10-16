using ApiFootball.Options;
using Microsoft.Extensions.Options;

namespace ApiFootball.Clients.Implementation;

public sealed class TrophiesClient(IHttpClientFactory factory, IOptions<ApiFootballOptions> options) : BaseClient(factory, options), ITrophiesClient
{
    protected override string Route => "trophies";

    public async Task<PagedResponse<Trophy>> GetTrophies(int? player = null, IEnumerable<int>? players = null, int? coach = null, IEnumerable<int>? coachs = null,
        CancellationToken cancellationToken = default)
    {
        var query = BuildQueryString((nameof(player), player), (nameof(players), JoinIds(players)), (nameof(coach), coach), (nameof(coachs), JoinIds(coachs)));
        return await GetAsync<PagedResponse<Trophy>>(query, cancellationToken);
    }

    private static string? JoinIds(IEnumerable<int>? ids) => ids is null ? null : string.Join("-", ids);
}
