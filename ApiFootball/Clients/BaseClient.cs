using System.Globalization;
using System.Web;
using ApiFootball.Options;
using ApiFootball.Quota;
using Microsoft.Extensions.Options;

namespace ApiFootball.Clients;

public abstract class BaseClient(IHttpClientFactory factory, IOptions<ApiFootballOptions> options)
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        IncludeFields = false,
        PropertyNameCaseInsensitive = true
    };

    private readonly HttpClient _httpClient = factory.CreateClient(ApiFootballGlobals.HttpClientName);

    protected abstract string Route { get; }


    protected async Task<TResponse> GetAsync<TResponse>(
        string url,
        CancellationToken cancellationToken = default)
        where TResponse : BaseResponse
    {
        using var response = await _httpClient.GetAsync(
            url, cancellationToken);

        var quota = ApiFootballQuota.FromHeaders(response.Headers);

        TResponse result;

        try
        {
            await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            result = await DeserializeAsync<TResponse>(stream, cancellationToken);
        }
        catch (JsonException) when (!response.IsSuccessStatusCode)
        {
            response.EnsureSuccessStatusCode();
            throw;
        }

        result.Quota = quota;

        if (!response.IsSuccessStatusCode)
        {
            result.EnsureSuccess(response.StatusCode);
            response.EnsureSuccessStatusCode();
        }

        if (options.Value.ThrowOnError)
            result.EnsureSuccess(response.StatusCode);

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

    private async Task<TResponse> DeserializeAsync<TResponse>(
        Stream stream,
        CancellationToken cancellationToken = default)
        where TResponse : BaseResponse
    {
        return await JsonSerializer.DeserializeAsync<TResponse>(stream, SerializerOptions, cancellationToken)
               ?? throw new JsonException(
                   "Could not deserialize API-FOOTBALL response.");
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

        var queryString = queryStringBuilder.ToString() ??
                          throw new InvalidOperationException("Unable to build query string");
        return queryString;
    }

    private static string FormatQueryValue(object? value)
    {
        return value switch
        {
            null => string.Empty,
            DateOnly dateOnly => dateOnly.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
            bool boolean => boolean ? "true" : "false",
            LeagueType type => type switch
            {
                LeagueType.League => "league",
                LeagueType.Cup => "cup",
                _ => throw new ArgumentOutOfRangeException(nameof(value))
            },
            IFormattable formattable => formattable.ToString(null, CultureInfo.InvariantCulture),
            _ => value.ToString() ?? string.Empty
        };
    }
}