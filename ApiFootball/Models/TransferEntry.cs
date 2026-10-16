namespace ApiFootball.Models;

public sealed class TransferEntry
{
    public DateOnly? Date { get; init; }
    public string? Type { get; init; }
    public TransferTeams? Teams { get; init; }
}