using ApiFootball.Options;
using Microsoft.Extensions.Options;

namespace ApiFootball.Clients.Implementation;

public sealed class StatusClient(IHttpClientFactory factory, IOptions<ApiFootballOptions> options)
    : BaseClient(factory, options), IStatusClient
{
    protected override string Route => "status";

    public Task<ObjectResponse<ApiStatusResponse>> GetStatus(CancellationToken cancellationToken = default)
    {
        return GetAsync<ObjectResponse<ApiStatusResponse>>(BuildQueryString(), cancellationToken);
    }
}