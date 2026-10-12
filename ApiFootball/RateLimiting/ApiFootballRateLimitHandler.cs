namespace ApiFootball.RateLimiting;

internal sealed class ApiFootballRateLimitHandler(ApiFootballRateLimiter limiter) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        using var lease = await limiter.AcquireAsync(cancellationToken);

        if (lease is {IsAcquired: false })
            throw new InvalidOperationException("API-Football rate limit exceeded and request queue is full.");

        return await base.SendAsync(request, cancellationToken);
    }
}