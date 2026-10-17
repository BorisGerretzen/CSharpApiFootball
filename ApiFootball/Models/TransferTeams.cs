namespace ApiFootball.Models;

public sealed class TransferTeams
{
    [JsonPropertyName("in")] public TeamReference? In { get; init; }

    public TeamReference? Out { get; init; }
}