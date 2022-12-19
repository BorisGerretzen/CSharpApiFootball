using Newtonsoft.Json;

namespace ApiFootball; 

public abstract class BaseClient {
    protected readonly HttpClient HttpClient;

    protected readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings {
        ContractResolver = new PrivateResolver(),
        ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
    };
    
    public BaseClient(HttpClient httpClient) {
        HttpClient = httpClient;
    }

    protected static string BuildQueryString(params (string key, object? value)[] args) {
        var queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
        
        foreach (var (key, value) in args) {
            if(value is null || string.IsNullOrEmpty(value.ToString())) continue;
            queryString.Add(key, value.ToString());
        }

        return "?"+queryString ?? throw new InvalidOperationException("Unable to build query string");
    }
}