namespace ApiFootball.Models.Responses;

public class BaseResponse<T>
{
    public required string Get { get; init; }

    [JsonConverter(typeof(DictionaryOrArrayConverter<string, string>))]
    public required Dictionary<string, string> Parameters { get; init; }

    [JsonConverter(typeof(DictionaryOrArrayConverter<string, string>))]
    public required Dictionary<string, string> Errors { get; init; }

    [JsonIgnore] public bool IsSuccess => Errors.Count == 0;

    public required int Results { get; init; }
    public required Paging Paging { get; init; }
    public required List<T> Response { get; init; }

    /// <summary>
    ///     Throws an exception if there are any errors in the response.
    /// </summary>
    /// <exception cref="ApiFootballException">If <see cref="Errors" /> is populated.</exception>
    public void EnsureSuccess()
    {
        if (!IsSuccess) throw new ApiFootballException(Errors.Select(kvp => new ApiFootballError(kvp.Key, kvp.Value)).ToList());
    }
}