namespace ApiFootball.Models;

public sealed class Fixture
{
    public int Id { get; init; }
    public string? Referee { get; init; }
    public required string Timezone { get; init; }
    public required DateTimeOffset Date { get; init; }
    public long Timestamp { get; init; }
    public required Periods Periods { get; init; }
    public required Venue Venue { get; init; }
    public required Status Status { get; init; }
}