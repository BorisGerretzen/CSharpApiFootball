namespace ApiFootball.Models;

public sealed class ApiRequestStatus
{
    public int? Current { get; init; }
    [JsonPropertyName("limit_day")]
    public int? LimitDay { get; init; }
}