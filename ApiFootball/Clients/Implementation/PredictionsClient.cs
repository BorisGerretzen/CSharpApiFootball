using ApiFootball.Options;
using Microsoft.Extensions.Options;

namespace ApiFootball.Clients.Implementation;

public sealed class PredictionsClient(IHttpClientFactory factory, IOptions<ApiFootballOptions> options)
    : BaseClient(factory, options), IPredictionsClient
{
    protected override string Route => "predictions";

    public async Task<PagedResponse<PredictionResponse>> GetPredictions(int fixture,
        CancellationToken cancellationToken = default)
    {
        var query = BuildQueryString((nameof(fixture), fixture));
        return await GetAsync<PagedResponse<PredictionResponse>>(query, cancellationToken);
    }
}