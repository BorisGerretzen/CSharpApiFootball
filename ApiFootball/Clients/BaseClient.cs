using System.Web;

namespace ApiFootball.Clients;

public abstract class BaseClient(IHttpClientFactory factory)
{
    protected readonly HttpClient HttpClient = factory.CreateClient(Globals.HttpClientName);

    protected readonly JsonSerializerOptions SerializerOptions = new()
    {
        IncludeFields = false,
        PropertyNameCaseInsensitive = true
    };

    protected abstract string Route { get; }

    protected string BuildQueryString(params (string key, object? value)[] args)
    {
        var queryString = BuildOnlyQueryString(args);
        return string.IsNullOrEmpty(queryString) ? Route : $"{Route}?{queryString}";
    }

    protected string BuildQueryString(string path, params (string key, object? value)[] args)
    {
        var queryString = BuildOnlyQueryString(args);
        return string.IsNullOrEmpty(queryString) ? Route + path : $"{Route}{path}?{queryString}";
    }

    private static string BuildOnlyQueryString(params (string key, object? value)[] args)
    {
        var queryStringBuilder = HttpUtility.ParseQueryString(string.Empty);

        foreach (var (key, value) in args)
        {
            if (value is null || string.IsNullOrEmpty(value.ToString())) continue;
            queryStringBuilder.Add(key, value.ToString());
        }

        var queryString = queryStringBuilder.ToString() ?? throw new InvalidOperationException("Unable to build query string");
        return queryString;
    }
}