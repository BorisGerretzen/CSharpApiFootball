namespace ApiFootball.Models;

public sealed class PredictionLastFive
{
    public string? Form { get; init; }
    public string? Att { get; init; }
    public string? Def { get; init; }

    [JsonPropertyName("goals")] public PredictionGoalSummary? Goals { get; init; }
}