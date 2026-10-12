using System.Threading.RateLimiting;

namespace ApiFootball.RateLimiting;

internal sealed class ApiFootballRateLimiter : IDisposable
{
    private readonly List<RateLimiter> _limiters = [];
    private readonly RateLimiter? _combined;

    public ApiFootballRateLimiter(ApiFootballRateLimiterOptions options)
    {
        options.Validate();

        var queueLimit = options.MaxQueuedRequests ?? int.MaxValue;
        const QueueProcessingOrder order = QueueProcessingOrder.OldestFirst;

        if (options.MaxConcurrentRequests is { } concurrent)
        {
            _limiters.Add(new ConcurrencyLimiter(
                new ConcurrencyLimiterOptions
                {
                    PermitLimit = concurrent,
                    QueueLimit = queueLimit,
                    QueueProcessingOrder = order
                }));
        }

        if (options.RequestsPerSecond is { } perSecond)
        {
            _limiters.Add(new SlidingWindowRateLimiter(
                new SlidingWindowRateLimiterOptions
                {
                    PermitLimit = perSecond,
                    Window = TimeSpan.FromSeconds(1),
                    SegmentsPerWindow = 10,
                    QueueLimit = queueLimit,
                    QueueProcessingOrder = order,
                    AutoReplenishment = true
                }));
        }

        if (options.RequestsPerMinute is { } perMinute)
        {
            _limiters.Add(new SlidingWindowRateLimiter(
                new SlidingWindowRateLimiterOptions
                {
                    PermitLimit = perMinute,
                    Window = TimeSpan.FromMinutes(1),
                    SegmentsPerWindow = 60,
                    QueueLimit = queueLimit,
                    QueueProcessingOrder = order,
                    AutoReplenishment = true
                }));
        }

        _combined = _limiters.Count switch
        {
            0 => null,
            1 => _limiters[0],
            _ => RateLimiter.CreateChained(_limiters.ToArray())
        };
    }

    public async ValueTask<RateLimitLease?> AcquireAsync(
        CancellationToken cancellationToken)
    {
        if (_combined is null)
            return null;

        return await _combined.AcquireAsync(
            1, cancellationToken);
    }

    public void Dispose()
    {
        if (_limiters.Count > 1)
            _combined?.Dispose();

        foreach (var limiter in _limiters)
            limiter.Dispose();
    }
}