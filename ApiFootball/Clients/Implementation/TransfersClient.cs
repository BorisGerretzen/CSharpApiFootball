using ApiFootball.Options;
using Microsoft.Extensions.Options;

namespace ApiFootball.Clients.Implementation;

public sealed class TransfersClient(IHttpClientFactory factory, IOptions<ApiFootballOptions> options)
    : BaseClient(factory, options), ITransfersClient
{
    protected override string Route => "transfers";

    public async Task<PagedResponse<TransferHistory>> GetTransfers(int? player = null, int? team = null,
        CancellationToken cancellationToken = default)
    {
        var query = BuildQueryString((nameof(player), player), (nameof(team), team));
        return await GetAsync<PagedResponse<TransferHistory>>(query, cancellationToken);
    }
}