using System.Net.Http.Headers;

namespace ApiFootball.Quota;

public sealed record ApiFootballQuota(
    int? DailyLimit,
    int? DailyRemaining,
    int? MinuteLimit,
    int? MinuteRemaining)
{
    public static ApiFootballQuota FromHeaders(HttpResponseHeaders headers)
    {
        return new ApiFootballQuota(
            Read(headers, "x-ratelimit-requests-limit"),
            Read(headers, "x-ratelimit-requests-remaining"),
            Read(headers, "X-RateLimit-Limit"),
            Read(headers, "X-RateLimit-Remaining"));

        static int? Read(HttpResponseHeaders headers, string name)
        {
            if (!headers.TryGetValues(name, out var values)) return null;

            var valueString = values.FirstOrDefault();
            if (!int.TryParse(valueString, out var value)) return null;

            return value;
        }
    }
}