namespace ApiFootball.Models;

public sealed class PredictionResponse
{
    public PredictionDetails? Predictions { get; init; }
    public TeamStatisticsLeague? League { get; init; }
    public PredictionTeams? Teams { get; init; }
    public PredictionComparison? Comparison { get; init; }
    public List<FixturesResponse>? H2h { get; init; }
}