namespace ApiFootball.Models;

/// <summary>Statistical summary returned by the teams/statistics endpoint.</summary>
public sealed class TeamStatisticsResponse
{
    public TeamStatisticsLeague? League { get; init; }
    public TeamReference? Team { get; init; }
    public string? Form { get; init; }
    public TeamStatisticsFixtures? Fixtures { get; init; }
    public TeamStatisticsGoals? Goals { get; init; }
    public TeamStatisticsBiggest? Biggest { get; init; }

    [JsonPropertyName("clean_sheet")] public TeamStatisticsCount? CleanSheet { get; init; }

    [JsonPropertyName("failed_to_score")] public TeamStatisticsCount? FailedToScore { get; init; }

    public TeamStatisticsPenalty? Penalty { get; init; }
    public List<TeamStatisticsLineup>? Lineups { get; init; }
    public Dictionary<string, Dictionary<string, TeamStatisticsCardCount>>? Cards { get; init; }
}