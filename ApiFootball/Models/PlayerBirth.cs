namespace ApiFootball.Models;

public sealed class PlayerBirth
{
    public DateOnly? Date { get; init; }
    public string? Place { get; init; }
    public string? Country { get; init; }
}