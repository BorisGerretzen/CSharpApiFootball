using System.Web;
using Newtonsoft.Json;

namespace ApiFootball;

public abstract class BaseClient {
    protected readonly HttpClient HttpClient;

    protected readonly JsonSerializerSettings SerializerSettings = new() {
        ContractResolver = new PrivateResolver(),
        ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
    };

    public BaseClient(IHttpClientFactory factory) {
        HttpClient = factory.CreateClient(DependencyInjectionExtensions.HttpClientName);
    }

    protected abstract string Route { get; }

    protected string BuildQueryString(params (string key, object? value)[] args) {
        var queryString = HttpUtility.ParseQueryString(string.Empty);

        foreach (var (key, value) in args) {
            if (value is null || string.IsNullOrEmpty(value.ToString())) continue;
            queryString.Add(key, value.ToString());
        }

        return Route + "?" + (queryString ?? throw new InvalidOperationException("Unable to build query string"));
    }
}