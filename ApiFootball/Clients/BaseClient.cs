using System.Web;
using ApiFootball.Options;
using ApiFootball.Quota;
using Microsoft.Extensions.Options;

namespace ApiFootball.Clients;

public abstract class BaseClient(IHttpClientFactory factory, IOptions<ApiFootballOptions> options)
{
    private readonly HttpClient HttpClient = factory.CreateClient(ApiFootballGlobals.HttpClientName);

    protected readonly JsonSerializerOptions SerializerOptions = new()
    {
        IncludeFields = false,
        PropertyNameCaseInsensitive = true
    };

    protected abstract string Route { get; }

    protected async Task<BaseResponse<T>> DeserializeAsync<T>(Stream stream, CancellationToken cancellationToken = default)
    {
        var deserialized = await JsonSerializer.DeserializeAsync<BaseResponse<T>>(stream, SerializerOptions, cancellationToken);
        if (deserialized is null) throw new NullReferenceException("Could not deserialize response.");
        if (options.Value.ThrowOnError) deserialized.EnsureSuccess();
        return deserialized;
    }
    
    protected async Task<BaseResponse<T>> GetAsync<T>(string url, CancellationToken cancellationToken = default)
    {
        using var response = await HttpClient.GetAsync(
            url,
            HttpCompletionOption.ResponseHeadersRead,
            cancellationToken);

        var quota = ApiFootballQuota.FromHeaders(
            response.Headers);

        response.EnsureSuccessStatusCode();

        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);

        var result = await DeserializeAsync<T>(stream, cancellationToken);
        result.Quota = quota;
        
        return result;
    }

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
            var formattedValue = FormatQueryValue(value);
            if (string.IsNullOrEmpty(formattedValue)) continue;
            queryStringBuilder.Add(key, formattedValue);
        }

        var queryString = queryStringBuilder.ToString() ?? throw new InvalidOperationException("Unable to build query string");
        return queryString;
    }
    
    private static string FormatQueryValue(object? value)
    {
        return value switch
        {
            null => string.Empty,
            DateOnly dateOnly => dateOnly.ToString("yyyy-MM-dd"),
            bool boolean => boolean ? "true" : "false",
            IFormattable formattable => formattable.ToString(null, System.Globalization.CultureInfo.InvariantCulture),
            _ => value.ToString() ?? string.Empty
        };
    }
}