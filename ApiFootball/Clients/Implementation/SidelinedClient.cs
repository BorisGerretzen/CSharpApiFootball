using ApiFootball.Options;
using Microsoft.Extensions.Options;

namespace ApiFootball.Clients.Implementation;

public sealed class SidelinedClient(IHttpClientFactory factory, IOptions<ApiFootballOptions> options)
    : BaseClient(factory, options), ISidelinedClient
{
    protected override string Route => "sidelined";

    public async Task<PagedResponse<SidelinedRecord>> GetSidelined(int? player = null, IEnumerable<int>? players = null,
        int? coach = null, IEnumerable<int>? coachs = null,
        CancellationToken cancellationToken = default)
    {
        var query = BuildQueryString((nameof(player), player), (nameof(players), JoinIds(players)),
            (nameof(coach), coach), (nameof(coachs), JoinIds(coachs)));
        return await GetAsync<PagedResponse<SidelinedRecord>>(query, cancellationToken);
    }

    private static string? JoinIds(IEnumerable<int>? ids)
    {
        return ids is null ? null : string.Join("-", ids);
    }
}