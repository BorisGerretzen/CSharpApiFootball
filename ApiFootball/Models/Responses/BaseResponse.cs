using ApiFootball.Json;
using ApiFootball.Quota;

namespace ApiFootball.Models.Responses;


public abstract class BaseResponse
{
    public required string Get { get; init; }

    [JsonConverter(typeof(DictionaryOrArrayConverter<string, string>))]
    public required Dictionary<string, string> Parameters { get; init; }

    [JsonConverter(typeof(DictionaryOrArrayConverter<string, string>))]
    public required Dictionary<string, string> Errors { get; init; }

    public required int Results { get; init; }

    [JsonIgnore]
    public bool IsSuccess => Errors.Count == 0;

    [JsonIgnore]
    public ApiFootballQuota? Quota { get; internal set; }

    /// <summary>
    /// Throws an exception if the API response contains errors.
    /// </summary>
    public void EnsureSuccess()
    {
        if (!IsSuccess)
        {
            var errors = Errors.Select(kvp => new ApiFootballError(kvp.Key, kvp.Value)).ToList();
            throw new ApiFootballException(errors);
        }
    }
}
