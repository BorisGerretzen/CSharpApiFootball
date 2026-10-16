namespace ApiFootball.Models;

public sealed class SidelinedRecord
{
    public string? Type { get; init; }
    public DateOnly? Start { get; init; }
    public DateOnly? End { get; init; }
}