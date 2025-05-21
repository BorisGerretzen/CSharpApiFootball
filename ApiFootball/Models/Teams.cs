namespace ApiFootball.Models;

public sealed class Teams
{
    public required Team Home { get; init; }
    public required Team Away { get; init; }
}