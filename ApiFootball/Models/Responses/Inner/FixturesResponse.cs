namespace ApiFootball.Models.Responses.Inner;

public sealed class FixturesResponse
{
    public required Fixture Fixture { get; init; }
    public required League League { get; init; }
    public required Teams Teams { get; init; }
    public required Goals Goals { get; init; }
    public required Score Score { get; init; }
}