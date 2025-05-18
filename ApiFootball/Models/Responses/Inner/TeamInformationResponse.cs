namespace ApiFootball.Models.Responses.Inner;

public sealed class TeamInformationResponse
{
    public required Team Team { get; init; }
    public required Venue Venue { get; init; }
}