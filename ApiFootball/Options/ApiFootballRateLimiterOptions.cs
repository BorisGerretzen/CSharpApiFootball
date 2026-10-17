namespace ApiFootball.Options;

public class ApiFootballRateLimiterOptions
{
    /// <summary>
    ///     Maximum number of requests per minute, if null, no limit is enforced.
    ///     If set must be greater than 0.
    /// </summary>
    public int? RequestsPerMinute { get; set; } = 10;

    /// <summary>
    ///     Maximum number of requests per second, if null, no limit is enforced.
    ///     If set must be greater than 0.
    /// </summary>
    public int? RequestsPerSecond { get; set; }

    /// <summary>
    ///     Maximum number of requests that can be queued when the rate limit is reached. If null, no limit is enforced.
    ///     If set must be greater than or equal to 0.
    /// </summary>
    public int? MaxQueuedRequests { get; set; } = 100;

    /// <summary>
    ///     Maximum number of concurrent requests that can be made at the same time. If null, no limit is enforced.
    ///     If set must be greater than 0.
    /// </summary>
    public int? MaxConcurrentRequests { get; set; }

    /// <summary>
    ///     Rate limiter options for the Free plan of api-football. Does not enforce daily rate limits.
    ///     Tweak <see cref="MaxQueuedRequests" /> and <see cref="MaxConcurrentRequests" /> to your needs.
    /// </summary>
    public static ApiFootballRateLimiterOptions FreePlan => new()
    {
        RequestsPerMinute = 10,
        RequestsPerSecond = null,
        MaxQueuedRequests = 100,
        MaxConcurrentRequests = 10
    };

    /// <summary>
    ///     Rate limiter options for the Pro plan of api-football. Does not enforce daily rate limits.
    ///     Tweak <see cref="MaxQueuedRequests" /> and <see cref="MaxConcurrentRequests" /> to your needs.
    /// </summary>
    public static ApiFootballRateLimiterOptions ProPlan => new()
    {
        RequestsPerMinute = 300,
        RequestsPerSecond = 5,
        MaxQueuedRequests = 300,
        MaxConcurrentRequests = 5
    };

    /// <summary>
    ///     Rate limiter options for the Ultra plan of api-football. Does not enforce daily rate limits.
    ///     Tweak <see cref="MaxQueuedRequests" /> and <see cref="MaxConcurrentRequests" /> to your needs.
    /// </summary>
    public static ApiFootballRateLimiterOptions UltraPlan => new()
    {
        RequestsPerMinute = 450,
        RequestsPerSecond = 7,
        MaxQueuedRequests = 450,
        MaxConcurrentRequests = 7
    };

    /// <summary>
    ///     Rate limiter options for the Mega plan of api-football. Does not enforce daily rate limits.
    ///     Tweak <see cref="MaxQueuedRequests" /> and <see cref="MaxConcurrentRequests" /> to your needs.
    /// </summary>
    public static ApiFootballRateLimiterOptions MegaPlan => new()
    {
        RequestsPerMinute = 900,
        RequestsPerSecond = 15,
        MaxQueuedRequests = 900,
        MaxConcurrentRequests = 15
    };


    internal void Validate()
    {
        if (RequestsPerMinute <= 0)
            throw new ArgumentOutOfRangeException(nameof(RequestsPerMinute),
                "RequestsPerMinute must be greater than 0.");
        if (RequestsPerSecond is <= 0)
            throw new ArgumentOutOfRangeException(nameof(RequestsPerSecond),
                "RequestsPerSecond must be greater than 0.");
        if (MaxQueuedRequests is < 0)
            throw new ArgumentOutOfRangeException(nameof(MaxQueuedRequests),
                "MaxQueuedRequests must be greater than or equal to 0.");
        if (MaxConcurrentRequests is <= 0)
            throw new ArgumentOutOfRangeException(nameof(MaxConcurrentRequests),
                "MaxConcurrentRequests must be greater than 0.");
    }
}