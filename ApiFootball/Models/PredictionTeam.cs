namespace ApiFootball.Models;

public sealed class PredictionTeam : TeamReference
{
    [JsonPropertyName("last_5")]
    public PredictionLastFive? Last5 { get; init; }
}