using ApiFootball.Clients.Implementation;
using ApiFootball.Clients.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace ApiFootball;

public static class DependencyInjectionExtensions {
    /// <summary>
    ///     Adds the ApiFootball api clients to the DI container.
    ///     <list type="bullet">
    ///         <item><see cref="IFixturesClient" /> to consume from the fixtures endpoint</item>
    ///         <item><see cref="ICountriesClient" /> to consume from the countries endpoint</item>
    ///         <item><see cref="ILeaguesClient" /> to consume from the leagues endpoint</item>
    ///         <item><see cref="ITimezoneClient" /> to consume from the timezone endpoint</item>
    ///     </list>
    /// </summary>
    /// <param name="services">DI service collection</param>
    /// <param name="apiKey">Your api-football api key</param>
    /// <param name="apiUrl">Base url of api-football, use the trailing slash</param>
    /// <returns></returns>
    public static IHttpClientBuilder AddApiFootball(this IServiceCollection services, string apiKey, string apiUrl = Globals.ApiUrl) {
        services.AddSingleton<IFixturesClient, FixturesClient>();
        services.AddSingleton<ICountriesClient, CountriesClient>();
        services.AddSingleton<ILeaguesClient, LeaguesClient>();
        services.AddSingleton<ITimezoneClient, TimezoneClient>();
        services.AddSingleton<ITeamsClient, TeamsClient>();

        return services.AddHttpClient(Globals.HttpClientName, client => {
            client.DefaultRequestHeaders.Add("x-rapidapi-key", apiKey);
            client.DefaultRequestHeaders.Add("x-rapidapi-host", apiUrl);
            client.BaseAddress = new Uri(apiUrl);
        });
    }
}