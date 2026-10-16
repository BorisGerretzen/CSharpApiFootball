namespace ApiFootball.Models;

public sealed class OddsBookmaker
{
    public int Id { get; init; }
    public string? Name { get; init; }
    public List<OddsBet>? Bets { get; init; }
}