using Microsoft.Extensions.DependencyInjection;

namespace ApiFootball;

public static class DependencyInjectionExtensions
{
    public static IHttpClientBuilder RegisterFixturesClient(this IServiceCollection services, string apiKey, string apiUrl)
    {
        services.AddSingleton<IFixturesClient, FixturesClient>();

        return services.AddHttpClient<IFixturesClient>(client =>
        {
            client.DefaultRequestHeaders.Add("x-rapidapi-key", apiKey);
            client.DefaultRequestHeaders.Add("x-rapidapi-host", apiUrl);
        });
    }
}