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
    ///         <item><see cref="IStatusClient" /> to consume from the status endpoint</item>
    ///         <item><see cref="ITeamsClient" /> to consume from the teams endpoint</item>
    ///         <item><see cref="ITimezoneClient" /> to consume from the timezone endpoint</item>
    ///         <item><see cref="IVenuesClient" /> to consume from the venues endpoint</item>
    ///         <item><see cref="IPlayersClient" /> to consume from the players endpoints</item>
    ///         <item><see cref="ICoachesClient" /> to consume from the coach endpoint</item>
    ///         <item><see cref="IInjuriesClient" /> to consume from the injuries endpoint</item>
    ///         <item><see cref="IPredictionsClient" /> to consume from the predictions endpoint</item>
    ///         <item><see cref="ITransfersClient" /> to consume from the transfers endpoint</item>
    ///         <item><see cref="ITrophiesClient" /> to consume from the trophies endpoint</item>
    ///         <item><see cref="ISidelinedClient" /> to consume from the sidelined endpoint</item>
    ///         <item><see cref="IOddsClient" /> to consume from the odds endpoints</item>
    ///         <item><see cref="IWidgetsClient" /> to consume from the widgets endpoints</item>
    ///     </list>
    /// </summary>
    /// <param name="services">DI service collection</param>
    /// <param name="apiKey">Your api-football api key</param>
    /// <param name="apiUrl">Base url of api-football, use the trailing slash</param>
    public static IServiceCollection AddApiFootball(this IServiceCollection services, string apiKey,
        string apiUrl = ApiFootballGlobals.ApiUrl)
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
    ///         <item><see cref="IStatusClient" /> to consume from the status endpoint</item>
    ///         <item><see cref="ITeamsClient" /> to consume from the teams endpoint</item>
    ///         <item><see cref="ITimezoneClient" /> to consume from the timezone endpoint</item>
    ///         <item><see cref="IVenuesClient" /> to consume from the venues endpoint</item>
    ///         <item><see cref="IPlayersClient" /> to consume from the players endpoints</item>
    ///         <item><see cref="ICoachesClient" /> to consume from the coach endpoint</item>
    ///         <item><see cref="IInjuriesClient" /> to consume from the injuries endpoint</item>
    ///         <item><see cref="IPredictionsClient" /> to consume from the predictions endpoint</item>
    ///         <item><see cref="ITransfersClient" /> to consume from the transfers endpoint</item>
    ///         <item><see cref="ITrophiesClient" /> to consume from the trophies endpoint</item>
    ///         <item><see cref="ISidelinedClient" /> to consume from the sidelined endpoint</item>
    ///         <item><see cref="IOddsClient" /> to consume from the odds endpoints</item>
    ///     </list>
    /// </summary>
    /// <param name="services">DI service collection</param>
    /// <param name="configureOptions">Configurator for the <see cref="ApiFootballOptions" /></param>
    public static IServiceCollection AddApiFootball(this IServiceCollection services,
        Action<ApiFootballOptions> configureOptions)
    {
        var options = new ApiFootballOptions();
        configureOptions(options);

        services.AddSingleton(Microsoft.Extensions.Options.Options.Create(options));
        services.AddTransient<IFixturesClient, FixturesClient>();
        services.AddTransient<ICountriesClient, CountriesClient>();
        services.AddTransient<ILeaguesClient, LeaguesClient>();
        services.AddTransient<ITimezoneClient, TimezoneClient>();
        services.AddTransient<ITeamsClient, TeamsClient>();
        services.AddTransient<IVenuesClient, VenuesClient>();
        services.AddTransient<IStandingsClient, StandingsClient>();
        services.AddTransient<IStatusClient, StatusClient>();
        services.AddTransient<IPlayersClient, PlayersClient>();
        services.AddTransient<ICoachesClient, CoachesClient>();
        services.AddTransient<IInjuriesClient, InjuriesClient>();
        services.AddTransient<IPredictionsClient, PredictionsClient>();
        services.AddTransient<ITransfersClient, TransfersClient>();
        services.AddTransient<ITrophiesClient, TrophiesClient>();
        services.AddTransient<ISidelinedClient, SidelinedClient>();
        services.AddTransient<IOddsClient, OddsClient>();

        var typedApiUri = new Uri(options.ApiUrl);
        var httpClient = services.AddHttpClient(ApiFootballGlobals.HttpClientName, client =>
        {
            if (typedApiUri.Host.EndsWith(".rapidapi.com", StringComparison.OrdinalIgnoreCase))
            {
                client.DefaultRequestHeaders.Add("X-Rapidapi-Key", options.ApiKey);
                client.DefaultRequestHeaders.Add("X-Rapidapi-Host", typedApiUri.Host);
            }
            else
            {
                client.DefaultRequestHeaders.Add("x-apisports-key", options.ApiKey);
            }

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

        return services;
    }
}