namespace ApiFootball.Models;

public class League {
    public int Id { get; private set; }
    public string Name { get; private set; }
    public LeagueType Type { get; private set; }
    public string Logo { get; private set; }
    public string? Flag { get; private set; }
    public string? Round { get; private set; }
    public int? Season { get; private set; }
}

public enum LeagueType {
    League,
    Cup
}