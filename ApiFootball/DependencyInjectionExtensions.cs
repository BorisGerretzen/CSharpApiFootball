using ApiFootball.Clients.Implementation;
using Microsoft.Extensions.DependencyInjection;

namespace ApiFootball;

public static class DependencyInjectionExtensions
{
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
    /// <param name="apiKey">Your api-football api key</param>
    /// <param name="apiUrl">Base url of api-football, use the trailing slash</param>
    public static IHttpClientBuilder AddApiFootball(this IServiceCollection services, string apiKey, string apiUrl = Globals.ApiUrl)
    {
        services.AddSingleton<IFixturesClient, FixturesClient>();
        services.AddSingleton<ICountriesClient, CountriesClient>();
        services.AddSingleton<ILeaguesClient, LeaguesClient>();
        services.AddSingleton<ITimezoneClient, TimezoneClient>();
        services.AddSingleton<ITeamsClient, TeamsClient>();
        services.AddSingleton<IVenuesClient, VenuesClient>();
        services.AddSingleton<IStandingsClient, StandingsClient>();

        var typedApiUri = new Uri(apiUrl);
        return services.AddHttpClient(Globals.HttpClientName, client =>
        {
            client.DefaultRequestHeaders.Add("X-Rapidapi-Key", apiKey);
            client.DefaultRequestHeaders.Add("X-Rapidapi-Host", typedApiUri.Host);
            client.BaseAddress = typedApiUri;
        });
    }
}