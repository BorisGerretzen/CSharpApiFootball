namespace ApiFootball.Quota;

internal class ApiFootballQuotaHandler(ApiFootballQuotaMonitor quotaMonitor) : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = base.SendAsync(request, cancellationToken);
        quotaMonitor.Observe(response.Result);
        return response;
    }
}