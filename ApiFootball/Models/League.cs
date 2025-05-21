namespace ApiFootball.Models;

public sealed class League
{
    public int Id { get; init; }
    public required string Name { get; init; }
    public string? Country { get; init; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public LeagueType Type { get; init; }

    public required string Logo { get; init; }
    public string? Flag { get; init; }
    public string? Round { get; init; }
    public int? Season { get; init; }
    public List<List<Standing>>? Standings { get; init; }
}

public enum LeagueType
{
    League,
    Cup
}