namespace ApiFootball.Models;

public sealed class OddsBet
{
    public int Id { get; init; }
    public string? Name { get; init; }
    public List<OddsValue>? Values { get; init; }
}