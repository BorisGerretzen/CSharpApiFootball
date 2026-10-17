using ApiFootball.Json;

namespace ApiFootball.Models.Responses;

public sealed class ObjectResponse<T> : BaseResponse
    where T : class
{
    [JsonConverter(typeof(EmptyArrayAsNullConverterFactory))]
    public required T? Response { get; init; }

    public T GetResponse()
    {
        EnsureSuccess();
        return Response ??
               throw new JsonException("Api-Football returned a null response when a non-null response was expected.");
    }
}