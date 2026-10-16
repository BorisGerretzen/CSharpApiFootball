using ApiFootball.Options;
using Microsoft.Extensions.Options;

namespace ApiFootball.Clients.Implementation;

public sealed class InjuriesClient(IHttpClientFactory factory, IOptions<ApiFootballOptions> options) : BaseClient(factory, options), IInjuriesClient
{
    protected override string Route => "injuries";

    public async Task<PagedResponse<InjuryResponse>> GetInjuries(int? league = null, int? season = null, int? fixture = null, int? team = null, int? player = null,
        DateOnly? date = null, IEnumerable<int>? ids = null, string? timezone = null, CancellationToken cancellationToken = default)
    {
        var query = BuildQueryString((nameof(league), league), (nameof(season), season), (nameof(fixture), fixture), (nameof(team), team), (nameof(player), player),
            (nameof(date), date), (nameof(ids), JoinIds(ids)), (nameof(timezone), timezone));
        return await GetAsync<PagedResponse<InjuryResponse>>(query, cancellationToken);
    }

    private static string? JoinIds(IEnumerable<int>? ids) => ids is null ? null : string.Join("-", ids);
}
