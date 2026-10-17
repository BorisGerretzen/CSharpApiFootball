namespace ApiFootball.Models;

public sealed class TransferHistory
{
    public PlayerProfile? Player { get; init; }
    public DateTimeOffset? Update { get; init; }
    public List<TransferEntry>? Transfers { get; init; }
}