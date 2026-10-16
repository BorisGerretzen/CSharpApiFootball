namespace ApiFootball.Models;

public sealed class TeamStatisticsGoalSplit
{
    public TeamStatisticsCount? Total { get; init; }
    public TeamStatisticsAverage? Average { get; init; }
    public Dictionary<string, TeamStatisticsCardCount>? Minute { get; init; }
    [JsonPropertyName("under_over")]
    public Dictionary<string, JsonElement>? UnderOver { get; init; }
}