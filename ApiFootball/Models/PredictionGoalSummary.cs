namespace ApiFootball.Models;

public sealed class PredictionGoalSummary
{
    public PredictionGoalSplit? For { get; init; }
    public PredictionGoalSplit? Against { get; init; }
}