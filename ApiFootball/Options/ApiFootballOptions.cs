using Microsoft.Extensions.DependencyInjection;

namespace ApiFootball.Options;

public class ApiFootballOptions
{
    /// <summary>
    ///     Your api-football api key.
    /// </summary>
    public string ApiKey { get; set; } = string.Empty;

    /// <summary>
    ///     Base url of api-football, use the trailing slash.
    /// </summary>
    public string ApiUrl { get; set; } = ApiFootballGlobals.ApiUrl;

    /// <summary>
    ///     Whether to throw exceptions on errors.
    /// </summary>
    public bool ThrowOnError { get; set; } = false;

    /// <summary>
    ///     Rate limiter options for the api-football clients. Set to null to disable rate limiting.
    ///     Use one of the predefined options for the plans of api-football, or create your own. The predefined options are:
    ///     <ul>
    ///         <li><see cref="ApiFootballRateLimiterOptions.FreePlan" />; </li>
    ///         <li><see cref="ApiFootballRateLimiterOptions.ProPlan" />; </li>
    ///         <li><see cref="ApiFootballRateLimiterOptions.UltraPlan" />; </li>
    ///         <li><see cref="ApiFootballRateLimiterOptions.MegaPlan" />; </li>
    ///     </ul>
    /// </summary>
    public ApiFootballRateLimiterOptions? RateLimiterOptions { get; set; } = null;

    /// <summary>
    ///     Optional action to configure the HttpClient used by the api-football clients.
    /// </summary>
    public Action<IHttpClientBuilder>? ConfigureHttpClient { get; set; }
}