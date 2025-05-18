namespace ApiFootball.Models;

public sealed class Status
{
    public required string Long { get; init; }
    public required string Short { get; init; }
    public double? Elapsed { get; init; }
}