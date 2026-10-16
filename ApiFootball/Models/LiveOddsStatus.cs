namespace ApiFootball.Models;

public sealed class LiveOddsStatus
{
    public bool? Stopped { get; init; }
    public bool? Blocked { get; init; }
    public bool? Finished { get; init; }
}