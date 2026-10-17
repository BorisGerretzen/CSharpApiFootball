namespace ApiFootball.Quota;

internal class ApiFootballQuotaHandler(ApiFootballQuotaMonitor quotaMonitor) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken);
        quotaMonitor.Observe(response);
        return response;
    }
}