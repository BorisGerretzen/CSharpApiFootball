using ApiFootball.Clients.Implementation;
using ApiFootball.Options;
using ApiFootball.Quota;
using ApiFootball.RateLimiting;
using Microsoft.Extensions.DependencyInjection;

namespace ApiFootball;

public static class DependencyInjectionExtensions
{
    /// <summary>
    ///     Adds the ApiFootball api clients to the DI container.
    ///     Shortcut for <see cref="AddApiFootball(IServiceCollection, Action{ApiFootballOptions})" />
    ///     <list type="bullet">
    ///         <item><see cref="ICountriesClient" /> to consume from the countries endpoint</item>
    ///         <item><see cref="IFixturesClient" /> to consume from the fixtures endpoint</item>
    ///         <item><see cref="ILeaguesClient" /> to consume from the leagues endpoint</item>
    ///         <item><see cref="IStandingsClient" /> to consume from the standings endpoint</item>
    ///         <item><see cref="ITeamsClient" /> to consume from the teams endpoint</item>
    ///         <item><see cref="ITimezoneClient" /> to consume from the timezone endpoint</item>
    ///         <item><see cref="IVenuesClient" /> to consume from the venues endpoint</item>
    ///     </list>
    /// </summary>
    /// <param name="services">DI service collection</param>
    /// <param name="apiKey">Your api-football api key</param>
    /// <param name="apiUrl">Base url of api-football, use the trailing slash</param>
    public static IHttpClientBuilder AddApiFootball(this IServiceCollection services, string apiKey, string apiUrl = ApiFootballGlobals.ApiUrl)
    {
        return services.AddApiFootball(o =>
        {
            o.ApiUrl = apiUrl;
            o.ApiKey = apiKey;
        });
    }

    /// <summary>
    ///     Adds the ApiFootball api clients to the DI container.
    ///     <list type="bullet">
    ///         <item><see cref="ICountriesClient" /> to consume from the countries endpoint</item>
    ///         <item><see cref="IFixturesClient" /> to consume from the fixtures endpoint</item>
    ///         <item><see cref="ILeaguesClient" /> to consume from the leagues endpoint</item>
    ///         <item><see cref="IStandingsClient" /> to consume from the standings endpoint</item>
    ///         <item><see cref="ITeamsClient" /> to consume from the teams endpoint</item>
    ///         <item><see cref="ITimezoneClient" /> to consume from the timezone endpoint</item>
    ///         <item><see cref="IVenuesClient" /> to consume from the venues endpoint</item>
    ///     </list>
    /// </summary>
    /// <param name="services">DI service collection</param>
    /// <param name="configureOptions">Configurator for the <see cref="ApiFootballOptions" /></param>
    public static IHttpClientBuilder AddApiFootball(this IServiceCollection services, Action<ApiFootballOptions> configureOptions)
    {
        var options = new ApiFootballOptions();
        configureOptions(options);
        
        services.Configure(configureOptions);
        services.AddSingleton<IFixturesClient, FixturesClient>();
        services.AddSingleton<ICountriesClient, CountriesClient>();
        services.AddSingleton<ILeaguesClient, LeaguesClient>();
        services.AddSingleton<ITimezoneClient, TimezoneClient>();
        services.AddSingleton<ITeamsClient, TeamsClient>();
        services.AddSingleton<IVenuesClient, VenuesClient>();
        services.AddSingleton<IStandingsClient, StandingsClient>();
        
        var typedApiUri = new Uri(options.ApiUrl);
        var httpClient = services.AddHttpClient(ApiFootballGlobals.HttpClientName, client =>
        {
            client.DefaultRequestHeaders.Add("X-Rapidapi-Key", options.ApiKey);
            client.DefaultRequestHeaders.Add("X-Rapidapi-Host", typedApiUri.Host);
            client.BaseAddress = typedApiUri;
        });

        options.ConfigureHttpClient?.Invoke(httpClient);
        
        if (options.RateLimiterOptions != null)
        {
            services.AddSingleton<ApiFootballRateLimiter>(_ => new ApiFootballRateLimiter(options.RateLimiterOptions));
            services.AddTransient<ApiFootballRateLimitHandler>();
            httpClient.AddHttpMessageHandler<ApiFootballRateLimitHandler>();
        }

        services.AddSingleton<ApiFootballQuotaMonitor>();
        services.AddSingleton<IApiFootballQuotaMonitor>(sp => sp.GetRequiredService<ApiFootballQuotaMonitor>());
        services.AddTransient<ApiFootballQuotaHandler>();
        httpClient.AddHttpMessageHandler<ApiFootballQuotaHandler>();
        
        return httpClient;
    }
}