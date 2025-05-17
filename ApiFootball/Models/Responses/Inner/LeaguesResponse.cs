namespace ApiFootball.Models.Responses.Inner;

public sealed class LeaguesResponse
{
    public required League League { get; init; }
    public required Country Country { get; init; }
    public required List<Season> Seasons { get; init; }
}