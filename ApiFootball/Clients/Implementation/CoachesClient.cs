using ApiFootball.Options;
using Microsoft.Extensions.Options;

namespace ApiFootball.Clients.Implementation;

public sealed class CoachesClient(IHttpClientFactory factory, IOptions<ApiFootballOptions> options) : BaseClient(factory, options), ICoachesClient
{
    protected override string Route => "coachs";

    public async Task<PagedResponse<CoachProfile>> GetCoachs(int? id = null, int? team = null, string? search = null,
        CancellationToken cancellationToken = default)
    {
        var query = BuildQueryString((nameof(id), id), (nameof(team), team), (nameof(search), search));
        return await GetAsync<PagedResponse<CoachProfile>>(query, cancellationToken);
    }
}
