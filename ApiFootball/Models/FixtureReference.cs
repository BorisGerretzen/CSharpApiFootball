namespace ApiFootball.Models;

public sealed class FixtureReference
{
    public int? Id { get; init; }
    public string? Timezone { get; init; }
    public DateTimeOffset? Date { get; init; }
    public long? Timestamp { get; init; }
}