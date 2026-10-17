namespace ApiFootball.Models;

public sealed class PredictionComparison
{
    public PredictionComparisonSides? Form { get; init; }
    public PredictionComparisonSides? Att { get; init; }
    public PredictionComparisonSides? Def { get; init; }

    [JsonPropertyName("poisson_distribution")]
    public PredictionComparisonSides? PoissonDistribution { get; init; }

    public PredictionComparisonSides? H2h { get; init; }
    public PredictionComparisonSides? Goals { get; init; }
    public PredictionComparisonSides? Total { get; init; }
}