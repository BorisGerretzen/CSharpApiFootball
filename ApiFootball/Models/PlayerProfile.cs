namespace ApiFootball.Models;

public class PlayerProfile
{
    public int Id { get; init; }
    public string? Name { get; init; }
    public string? Firstname { get; init; }
    public string? Lastname { get; init; }
    public int? Age { get; init; }
    public PlayerBirth? Birth { get; init; }
    public string? Nationality { get; init; }
    public string? Height { get; init; }
    public string? Weight { get; init; }
    public bool? Injured { get; init; }
    public int? Number { get; init; }
    public string? Position { get; init; }
    public string? Photo { get; init; }
}