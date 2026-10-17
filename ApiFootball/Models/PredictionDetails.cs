namespace ApiFootball.Models;

public sealed class PredictionDetails
{
    public PredictionWinner? Winner { get; init; }

    [JsonPropertyName("win_or_draw")] public bool? WinOrDraw { get; init; }

    [JsonPropertyName("under_over")] public string? UnderOver { get; init; }

    public PredictionGoals? Goals { get; init; }
    public string? Advice { get; init; }
    public PredictionPercent? Percent { get; init; }
}