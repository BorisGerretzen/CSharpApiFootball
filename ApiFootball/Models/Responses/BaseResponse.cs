namespace ApiFootball.Models.Responses;

public class BaseResponse<T>
{
    public required string Get { get; init; }

    [JsonConverter(typeof(DictionaryOrArrayConverter<string, string>))]
    public required Dictionary<string, string> Parameters { get; init; }

    [JsonConverter(typeof(DictionaryOrArrayConverter<string, string>))]
    public required Dictionary<string, string> Errors { get; init; }

    public required int Results { get; init; }
    public required Paging Paging { get; init; }
    public required List<T> Response { get; init; }
}