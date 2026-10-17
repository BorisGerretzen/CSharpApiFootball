namespace ApiFootball.Models;

public sealed class ApiSubscriptionStatus
{
    public string? Plan { get; init; }
    public DateTimeOffset? End { get; init; }
    public bool? Active { get; init; }
}