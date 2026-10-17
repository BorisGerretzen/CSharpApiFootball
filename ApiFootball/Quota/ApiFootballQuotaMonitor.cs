namespace ApiFootball.Quota;

public interface IApiFootballQuotaMonitor
{
    /// <summary>
    ///     Shows the last received quota information.
    ///     Note that out of order responses may cause this value to be inaccurate.
    /// </summary>
    ApiFootballQuota? Current { get; }
}

internal class ApiFootballQuotaMonitor : IApiFootballQuotaMonitor
{
    private ApiFootballQuota? _current;

    public ApiFootballQuota? Current => Volatile.Read(ref _current);

    public void Observe(HttpResponseMessage response)
    {
        var quota = ApiFootballQuota.FromHeaders(response.Headers);

        if (quota.DailyLimit is null &&
            quota.DailyRemaining is null &&
            quota.MinuteLimit is null &&
            quota.MinuteRemaining is null)
            return;

        Interlocked.Exchange(ref _current, quota);
    }
}